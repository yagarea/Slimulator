using System;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Slimulator {
    public class Simulation {
        private Random random;
        private Space space;
        private AnimationBuffer _animationBuffer;
        private string outputVideoPath;
        private int tickCount;

        public Simulation(Space space, string seed, string outputVideoPath) {
            this.outputVideoPath = outputVideoPath;
            this.space = space;
            random = new Random(seed.GetHashCode());
            _animationBuffer = new AnimationBuffer(outputVideoPath, space.Height, space.Width, 30);
            _animationBuffer.AddFrame(space.ExportBitmap());
            tickCount = 0;
        }

        public void Tick() {
            _animationBuffer.AddFrame(space.ExportBitmap());
            //space.ExportBitmap().Save(tickCount.ToString() + ".png", ImageFormat.Png);
            //space.TextLog();
            tickCount++;
        }

        public void End(bool play=false) {
            _animationBuffer.Export();
            if (play) {
                ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "vlc", Arguments = outputVideoPath, }; 
                Process proc = new Process() { StartInfo = startInfo, };
                proc.Start();
            }
        }
    }
}