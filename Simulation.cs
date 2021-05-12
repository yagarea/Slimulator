using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Slimulator {
    public class Simulation {
        private Random          _randomizer;
        private Space           _space;
        private AnimationBuffer _animationBuffer;
        private string          _outputVideoPath;
        private int             _tickCount;
        private int             _ticksPerFrame;
        private int             _simTicks;

        public Simulation(Space space, string outputVideoPath, int simTicks, string seed = "HlenkaHelenka", int ticksPerFrame = 3, int frameRate = 60) {
            _outputVideoPath = outputVideoPath;
            _space           = space;
            _simTicks        = simTicks;
            _randomizer      = new Random(seed.GetHashCode());
            _animationBuffer = new AnimationBuffer(outputVideoPath, space.Height, space.Width, frameRate);
            _tickCount       = 0;
            _ticksPerFrame   = ticksPerFrame;
            _animationBuffer.AddFrame(space.ExportBitmap());
            Console.WriteLine($"Output file: {outputVideoPath}");
            Console.WriteLine(
                $"      Specs: FPS: {frameRate} TFC: {simTicks / ticksPerFrame} TimeSpan: {(simTicks / ticksPerFrame) / frameRate}");
        }

        private void Tick() {
            HashSet<Point> currentSlime = Slime.FindAllSlime(_space);
            PickRandomPoint(Slime.FindAllPossiblePlacesToMove(_space, currentSlime))
                .SetType(PointType.Slime);
            PickRandomPoint(Slime.FindAllPossibleSlimesToPerish(_space, currentSlime)).SetType(PointType.ExploredSpace);
            if ((_tickCount % _ticksPerFrame) == 0) _animationBuffer.AddFrame(_space.ExportBitmap());
            _tickCount++;
            _space.GetOlder();
            Console.SetCursorPosition(0, Console.CursorTop -1);
            Console.WriteLine($"Tick: {_tickCount}");
        }

        public int Start() {
            Console.WriteLine("Simulation started\n");
            while (_tickCount < _simTicks)
                Tick();

            return _tickCount;
        }

        public void End(bool play = false) {
            _animationBuffer.AddFrame(_space.ExportBitmap());
            _animationBuffer.Export();
            if (!play) return;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments      = Path.GetFullPath(_outputVideoPath),
                FileName       = "/usr/bin/vlc",
                CreateNoWindow = true
            };
            using Process p = Process.Start(startInfo);
            if (p == null) throw new Exception("Could not start VLC");
            p.WaitForExit();
        }

        private Point PickRandomPoint(HashSet<Point> points) {
            return points.ElementAt(_randomizer.Next(points.Count));
        }
    }
}