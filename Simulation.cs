using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;

namespace Slimulator {
    public class Simulation {
        private Random randomizer;
        private Space space;
        private AnimationBuffer _animationBuffer;
        private string outputVideoPath;
        private int tickCount;

        public Simulation(Space space, string seed, string outputVideoPath) {
            this.outputVideoPath = outputVideoPath;
            this.space = space;
            randomizer = new Random(seed.GetHashCode());
            _animationBuffer = new AnimationBuffer(outputVideoPath, space.Height, space.Width, 60);
            _animationBuffer.AddFrame(space.ExportBitmap());
            tickCount = 0;
        }

        public void Tick() {
            PickRandomPoint(Slime.FindAllPossiblePlacesToMove(space, Slime.FindAllSlime(space)))
                .SetType(PointType.Slime);
            
            _animationBuffer.AddFrame(space.ExportBitmap());
            tickCount++;
            Console.WriteLine($"Frame: {tickCount}");
        }

        public void End(bool play = false) {
            _animationBuffer.Export();
            if (play) {
                ProcessStartInfo startInfo = new ProcessStartInfo() {FileName = "vlc", Arguments = outputVideoPath,};
                Process proc = new Process() {StartInfo = startInfo,};
                proc.Start();
            }
        }

        private Point PickRandomPoint(HashSet<Point> points) {
            Point[] pointArray = points.ToArray();
            return pointArray[randomizer.Next(pointArray.Length)];
        }
    }
}