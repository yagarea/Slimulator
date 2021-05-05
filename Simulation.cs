using System;

namespace Slimulator {
    public class Simulation {
        private Random random;
        private Space space;
        private AnimationBuffer _animationBuffer;

        public Simulation(Space space, string seed, string outputVideoPath) {
            this.space = space;
            random = new Random(seed.GetHashCode());
            _animationBuffer = new AnimationBuffer(outputVideoPath, space.Height, space.Width, 30);
            _animationBuffer.AddFrame(space.ExportBitmap());
        }

        public void tick() {
            _animationBuffer.AddFrame(space.ExportBitmap());
        }

        public void End() {
            _animationBuffer.Export();
        }
    }
}