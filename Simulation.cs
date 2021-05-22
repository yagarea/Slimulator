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
        private HashSet<Point> _pointsToMoveTo;
        private HashSet<Point> _pointsToLeave;
        private int[][] _coordinatesOfSlimeAccessiblePoint;

        public Simulation(string inputFile, string outputVideoPath, SimulationSettings settings = null) {
            _settings = settings ?? SimulationSettings.DefaultSettings();
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
            string offset = "                   ";
            Console.WriteLine($"{offset}TTC: {settings.TotalCountOfSimulationTicks}");
            Console.WriteLine($"{offset}TFC: {settings.TotalCountOfSimulationTicks / settings.TicksPerFrame}");
            Console.WriteLine(
                $"{offset}Time: {(settings.TotalCountOfSimulationTicks / settings.TicksPerFrame) / settings.FrameRate}s");
        }


        public int Start() {
            Console.WriteLine("Simulation started\n");
            while (_tickCount < _settings.TotalCountOfSimulationTicks) {
                if(!Tick()) break;
            }

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
            _space = UpdateSpace(_space);
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
//---------------------------------------------------------------------------------------------------------------------        
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

        private Space UpdateSpace(Space currentSpace) {
            _pointsToLeave.Clear();
            _pointsToMoveTo.Clear();
            Space updatedSpace = new Space(currentSpace);

            int spaceThreadStrip = currentSpace.Width / _settings.ThreadCount;
            Task[] tasks = new Task[_settings.ThreadCount];
            
            for (int threadIndex = 0; threadIndex < _settings.ThreadCount - 1; threadIndex++) {
                tasks[threadIndex] = Task.Factory.StartNew(() =>
                        SpaceUpdatingThreadFunction(currentSpace, updatedSpace, threadIndex * spaceThreadStrip,
                            (threadIndex + 1) * spaceThreadStrip));
            }
            tasks[_settings.ThreadCount -1] = Task.Factory.StartNew(() =>
                SpaceUpdatingThreadFunction(currentSpace, updatedSpace, (_settings.ThreadCount - 1) * spaceThreadStrip,
                    _space.Width));

            Task.WaitAll(tasks);
            return updatedSpace;
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

        private bool IsReadyToBeLeft(Space s, Point p) {
            foreach (int shift in new[] {-1, 1}) {
                if (s.GetPointType(p.X + shift, p.Y) == PointType.Space ||
                    s.GetPointType(p.X, p.Y + shift) == PointType.Space) {
                    return true;
                }
            }

            return false;
        }

        private void SpaceUpdatingThreadFunction(Space currentSpace, Space updatedSpace, int stripStart, int stripEnd) {
            for (int x = stripStart; x < stripEnd; x++) {
                foreach (int y in _coordinatesOfSlimeAccessiblePoint[x]) {
                    Point currentPoint = currentSpace.GetPoint(x, y);
                    if (currentSpace.GetPointType(x, y) == PointType.Space) {
                        updatedSpace.GetPoint(x, y).SlimeAffinity =
                            (_settings.SlimeTimeAffinityMultiplier * currentPoint.Age) +
                            (_settings.SlimeOccurenceAffinityMultiplier *
                             OccurrenceInRadius(currentPoint, PointType.Space,
                                 _settings.SlimeAffinityRadius));
                        currentPoint.GetOlder();
                        if (IsAvailableForMovingIn(currentPoint))
                            _pointsToMoveTo.Add(updatedSpace.GetPoint(x, y));
                    }

                    if (currentSpace.GetPointType(x, y) == PointType.Slime) {
                        if (IsReadyToBeLeft(currentSpace, currentPoint))
                            _pointsToLeave.Add(updatedSpace.GetPoint(x, y));
                    }
                }
            }
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
    }
}