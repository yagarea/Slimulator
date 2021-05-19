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
        private string outputVideoPath;
        private int tickCount;
        private int ticksPerFrame;
        private int simTicks;

        public Simulation(Space space, string outputVideoPath, int simTicks, string seed = "HlenkaHelenka",
            int ticksPerFrame = 3, int frameRate = 60) {
            this.outputVideoPath = outputVideoPath;
            this.space = space;
            this.simTicks = simTicks;
            randomizer = new Random(seed.GetHashCode());
            _animationBuffer = new AnimationBuffer(outputVideoPath, space.Height, space.Width, frameRate);
            _animationBuffer.AddFrame(space.ExportBitmap());
            tickCount = 0;
            this.ticksPerFrame = ticksPerFrame;
            Console.WriteLine($"Output file: {outputVideoPath}");
            Console.WriteLine(
                $"      Specs: FPS: {frameRate} TFC:{simTicks / ticksPerFrame} Time:{(simTicks / ticksPerFrame) / frameRate}s");
        }

        private void Tick() {
            HashSet<Point> currentSlime = Slime.FindAllSlime(space);
            PickRandomPoint(Slime.FindAllPossiblePlacesToMove(space, currentSlime))
                .SetType(PointType.Slime);
            PickRandomPoint(Slime.FindAllPossibleSlimesToPerish(space, currentSlime)).SetType(PointType.Space);
            if (tickCount % ticksPerFrame == 0) _animationBuffer.AddFrame(space.ExportBitmap());
            tickCount++;
            space.GetOlder();
            Console.SetCursorPosition(0, Console.CursorTop -1);
            Console.WriteLine($"Tick: {tickCount}");
        }

        public int Start() {
            Console.WriteLine("Simulation started\n");
            while (tickCount < simTicks) {
                Tick();
            }
            return tickCount;
        }

        public void End(bool play = false) {
            _animationBuffer.AddFrame(space.ExportBitmap());
            _animationBuffer.Export();
            if (play) {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.Arguments = Path.GetFullPath(outputVideoPath);
                startInfo.FileName = "/usr/bin/vlc";
                startInfo.CreateNoWindow = true;
                using (Process p = Process.Start(startInfo)) {
                    p.WaitForExit();
                }
            }
        }

        private Point PickRandomPoint(HashSet<Point> points) {
            Point[] pointArray = points.ToArray();
            return pointArray[randomizer.Next(pointArray.Length)];
        }
        
    }
}