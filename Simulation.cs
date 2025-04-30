using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Slimulator {
    public class Simulation {
        private readonly Random _randomizer;
        private Space _space;
        private readonly AnimationBuffer _animationBuffer;
        private readonly SimulationSettings _settings;
        private int _tickCount;
        private readonly string _outputVideoPath;
        private readonly HashSet<Point> _pointsToMoveTo;
        private readonly HashSet<Point> _pointsToLeave;
        private int[][] _coordinatesOfSlimeAccessiblePoint;

        public Simulation(string inputFile, string outputVideoPath, SimulationSettings settings = null) {
            _settings = settings ?? SimulationSettings.Default();
            _outputVideoPath = outputVideoPath;
            Console.WriteLine($"      Output file: {outputVideoPath}");
            _space = new Space(inputFile);
            Debug.Assert(settings != null, nameof(settings) + " != null");
            _randomizer = new Random(settings.Seed.GetHashCode());
            _animationBuffer =
                new AnimationBuffer(outputVideoPath, _space.Height, _space.Width, this._settings.FrameRate);
            _animationBuffer.AddFrame(_space.ExportBitmap());
            _tickCount = 0;
            _pointsToLeave = new HashSet<Point>();
            _pointsToMoveTo = new HashSet<Point>();
            InitCoordinatesOfSlimeAccessiblePoint(_space);
            Console.WriteLine($"            Specs: FPS: {settings.FrameRate}");
            const string offset = "                   ";
            Console.WriteLine($"{offset}TTC: {settings.TotalCountOfSimulationTicks}");
            Console.WriteLine($"{offset}TFC: {settings.TotalCountOfSimulationTicks / settings.TicksPerFrame}");
            Console.WriteLine(
                $"{offset}Time: {(settings.TotalCountOfSimulationTicks / settings.TicksPerFrame) / settings.FrameRate}s");
        }


        public int Start() {
            Console.WriteLine($"Simulation started: {DateTime.Now:HH:mm:ss}\n");
            while (_tickCount < _settings.TotalCountOfSimulationTicks) {
                if (!Tick()) break;
            }

            Console.WriteLine($"Simulation ended: {DateTime.Now:HH:mm:ss}");
            return _tickCount;
        }

        public void End(bool play = false) {
            _animationBuffer.AddFrame(_space.ExportBitmap());
            _animationBuffer.Export();
            if (!play) return;
            ProcessStartInfo startInfo = new ProcessStartInfo {
                Arguments = Path.GetFullPath(_outputVideoPath), FileName = "/usr/bin/vlc", CreateNoWindow = true
            };
            using Process p = Process.Start(startInfo);
            p?.WaitForExit();
        }

        private bool Tick() {
            _space = UpdateSpace();
            if (_pointsToLeave.Count == 0 || _pointsToMoveTo.Count == 0) {
                Console.WriteLine("Simulation: Reached dead point");
                return false;
            }

            PickPoint(_pointsToMoveTo).SetType(PointType.Slime);
            PickPoint(_pointsToLeave).SetType(PointType.Space);
            _animationBuffer.AddFrame(_space.ExportBitmap());
            _tickCount++;

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine($"Tick: {_tickCount}");
            return true;
        }
// Simulation control
//-------------------------------------------------------------------------------------
// Simulation logic

        private void InitCoordinatesOfSlimeAccessiblePoint(Space s) {
            int count = 0;
            _coordinatesOfSlimeAccessiblePoint = new int[s.Width][];
            for (int x = 0; x < s.Width; x++) {
                List<int> columnBuffer = new List<int>();
                for (int y = 0; y < s.Height; y++) {
                    if (s.GetPointType(x, y) != PointType.Wall) {
                        columnBuffer.Add(y);
                        count++;
                    }
                }

                _coordinatesOfSlimeAccessiblePoint[x] = columnBuffer.ToArray();
            }

            Console.WriteLine($"Slime accessible points: {count}");
        }

        private Space UpdateSpace() {
            _pointsToLeave.Clear();
            _pointsToMoveTo.Clear();
            Space updatedSpace = new Space(_space);

            int spaceThreadStrip = _space.Width / _settings.ThreadCount;
            Task<Tuple<HashSet<Point>, HashSet<Point>>>[] tasks =
                new Task<Tuple<HashSet<Point>, HashSet<Point>>>[_settings.ThreadCount];

            for (int threadIndex = 0; threadIndex < _settings.ThreadCount - 1; threadIndex++) {
                tasks[threadIndex] = Task.Run(() => {
                    return SpaceStripUpdatingTask(updatedSpace, threadIndex * spaceThreadStrip,
                        (threadIndex + 1) * spaceThreadStrip);
                });
            }

            tasks[_settings.ThreadCount - 1] = Task.Run(() => SpaceStripUpdatingTask(updatedSpace,
                (_settings.ThreadCount - 1) * spaceThreadStrip,
                _space.Width));

            Task.WaitAll(tasks);
            foreach (Task<Tuple<HashSet<Point>, HashSet<Point>>> t in tasks) {
                _pointsToMoveTo.UnionWith(t.Result.Item1);
                _pointsToLeave.UnionWith(t.Result.Item2);
            }

            return updatedSpace;
        }


        private Tuple<HashSet<Point>, HashSet<Point>> SpaceStripUpdatingTask(Space updatedSpace, int stripStart,
            int stripEnd) {
            HashSet<Point> pointsToMoveTo = new HashSet<Point>();
            HashSet<Point> pointsToLeave = new HashSet<Point>();
            for (int x = stripStart; x < stripEnd; x++) {
                foreach (int y in _coordinatesOfSlimeAccessiblePoint[x]) {
                    Point currentPoint = _space.GetPoint(x, y);
                    if (currentPoint.GetType() == PointType.Space || currentPoint.GetType() == PointType.Food) {
                        updatedSpace.GetPoint(x, y).SlimeAffinity =
                            (_settings.SlimeTimeAffinityMultiplier * currentPoint.Age) +
                            (_settings.SlimeOccurenceAffinityMultiplier *
                             OccurrenceInRadius(currentPoint, PointType.Space,
                                 _settings.SlimeAffinityRadius));

                        if (IsAvailableForMovingIn(currentPoint))
                            pointsToMoveTo.Add(updatedSpace.GetPoint(x, y));
                    }

                    if (_space.GetPointType(x, y) == PointType.Slime) {
                        currentPoint.SlimeAffinity = _settings.SlimeTimeAffinityMultiplier * currentPoint.Age;
                        if (IsReadyToBeLeft(currentPoint))
                            pointsToLeave.Add(updatedSpace.GetPoint(x, y));
                    }

                    currentPoint.AgeStep();
                }
            }

            return new Tuple<HashSet<Point>, HashSet<Point>>(pointsToMoveTo, pointsToLeave);
        }


        private bool IsAvailableForMovingIn(Point p) {
            foreach (int shift in new[] {-1, 1}) {
                if (_space.GetPointType(p.X + shift, p.Y) == PointType.Slime ||
                    _space.GetPointType(p.X, p.Y + shift) == PointType.Slime) {
                    return true;
                }
            }

            return false;
        }

        private bool IsReadyToBeLeft(Point p) {
            foreach (int shift in new[] {-1, 1}) {
                if (_space.GetPointType(p.X + shift, p.Y) == PointType.Space ||
                    _space.GetPointType(p.X, p.Y + shift) == PointType.Space) {
                    return true;
                }
            }

            return false;
        }

        private int OccurrenceInRadius(Point p, PointType type, int radius) {
            int occurrence = 0;
            radius = Math.Abs(radius);
            for (int xShift = -radius; xShift <= radius; xShift++) {
                for (int yShift = -radius; yShift <= radius; yShift++) {
                    if (Space.DistanceOf(p, _space.GetPoint(p.X + xShift, p.Y + yShift)) <= radius) {
                        if (_space.GetPointType(p.X + xShift, p.Y + yShift) == type) {
                            occurrence++;
                        }
                    }
                }
            }

            return occurrence;
        }

        private Point PickPoint(IEnumerable<Point> pointSet) {
            HashSet<Point> selected = new HashSet<Point>();
            int max = int.MinValue;
            foreach (Point p in pointSet) {
                if (p.SlimeAffinity > max) {
                    selected.Clear();
                    max = p.SlimeAffinity;
                }

                if (p.SlimeAffinity == max) {
                    selected.Add(p);
                }
            }

            return GetRandomPoint(selected);
        }

        private Point GetRandomPoint(IReadOnlyCollection<Point> points) {
            return points.ElementAt(_randomizer.Next(points.Count));
        }

        private bool WouldBreakIfRemoved(Point p) {
            Boolean[,] surroundings = new bool[3, 3];
            Tuple<int, int> randomSlimePoint = null;
            int slimeCount = 0;
            for (int shiftX = -1; shiftX <= 1; shiftX++) {
                for (int shiftY = -1; shiftY <= 1; shiftY++) {
                    if (_space.GetPointType(p.X + shiftX, p.Y + shiftY) == PointType.Slime) {
                        randomSlimePoint ??= new Tuple<int, int>(1 + shiftX, 1 + shiftY);
                        surroundings[1 + shiftX, 1 + shiftY] = true;
                        slimeCount++;
                    }
                }
            }

            Queue<Tuple<int, int>> bsfQueue = new Queue<Tuple<int, int>>();
            bsfQueue.Enqueue(randomSlimePoint);
            while (bsfQueue.Count > 0) {
                Tuple<int, int> currentPoint = bsfQueue.Dequeue();
                for (int shiftX = -1; shiftX <= 1; shiftX++) {
                    for (int shiftY = -1; shiftY <= 1; shiftY++) {
                        if (-1 >= currentPoint.Item1 + shiftX || currentPoint.Item1 + shiftX >= 2 ||
                            -1 >= currentPoint.Item2 + shiftY || currentPoint.Item2 + shiftY >= 2) continue;
                        if (!surroundings[currentPoint.Item1 + shiftX, currentPoint.Item2 + shiftY]) continue;
                        bsfQueue.Enqueue(new Tuple<int, int>(currentPoint.Item1 + shiftX, currentPoint.Item2 + shiftY));
                    }
                }

                surroundings[currentPoint.Item1, currentPoint.Item2] = false;
                slimeCount--;
            }

            return slimeCount != 0;
        }
    }
}