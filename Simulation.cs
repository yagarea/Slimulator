using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Slimulator { 
    public class Simulation {
        private Random randomizer;
        private Space space;
        private AnimationBuffer _animationBuffer;
        private SimulationSettings settings;
        private int tickCount;
        private string outputVideoPath;

        public Simulation(string inputFile, string outputVideoPath, SimulationSettings settings = null) {
            this.settings = settings ?? SimulationSettings.DefaultSettings();
            space = new Space(inputFile);
            this.outputVideoPath = outputVideoPath;
            randomizer = new Random(settings.Seed.GetHashCode());
            _animationBuffer = new AnimationBuffer(outputVideoPath, space.Height, space.Width, this.settings.FrameRate);
            _animationBuffer.AddFrame(space.ExportBitmap());
            tickCount = 0;
            Console.WriteLine($"Output file:\t\t{outputVideoPath}");
                Console.Write($"      Specs:\n FPS: {settings.FrameRate} ");
            Console.Write($"TFC:{settings.TotalCountOfSimulationTicks / settings.TicksPerFrame}");
            Console.WriteLine($"Time:{(settings.TotalCountOfSimulationTicks / settings.TicksPerFrame) / settings.FrameRate}s");
        }

        private void Tick() {
            HashSet<Point> currentSlime = Slime.FindAllSlime(space);
            PickRandomPoint(Slime.FindAllPossiblePlacesToMove(space, currentSlime))
                .SetType(PointType.Slime);
            PickRandomPoint(Slime.FindAllPossibleSlimesToPerish(space, currentSlime)).SetType(PointType.Space);
            if (tickCount % settings.TicksPerFrame == 0) _animationBuffer.AddFrame(space.ExportBitmap());
            tickCount++;
            space.GetOlder();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine($"Tick: {tickCount}");
        }

        public int Start() {
            Console.WriteLine("Simulation started\n");
            while (tickCount < settings.TotalCountOfSimulationTicks) {
                Tick();
            }

            return tickCount;
        }

        public void End(bool play = false) {
            _animationBuffer.AddFrame(space.ExportBitmap());
            _animationBuffer.Export();
            if (!play) return;
            ProcessStartInfo startInfo = new ProcessStartInfo {
                Arguments = Path.GetFullPath(outputVideoPath), FileName = "/usr/bin/vlc", CreateNoWindow = true
            };
            using Process p = Process.Start(startInfo);
            p?.WaitForExit();
        }

        private Point PickRandomPoint(HashSet<Point> points) {
            return points.ElementAt(randomizer.Next(points.Count));
        }
    }
}