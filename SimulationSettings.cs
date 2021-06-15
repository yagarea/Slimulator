namespace Slimulator {
    public class SimulationSettings {
        private int _totalCountOfSimulationTicks;
        private int _ticksPerFrame;
        private int _frameRate;

        private string _seed;
        private int _threadCount;

        private int _slimeAffinityRadius;
        private int _slimeOccurenceAffinityMultiplier;
        private int _slimeTimeAffinityMultiplier;

        public SimulationSettings(
            int totalCountOfSimulationTicks = 70_000,
            int ticksPerFrame = 20,
            int frameRate = 60,
            string seed = "HlenkaHelenka",
            int threadCount = 1,
            int slimeAffinityRadius = 4,
            int slimeOccurenceAffinityMultiplier = 1,
            int slimeTimeAffinityMultiplier = 10) {
            _totalCountOfSimulationTicks = totalCountOfSimulationTicks;
            _ticksPerFrame = ticksPerFrame;
            _frameRate = frameRate;
            _seed = seed;
            _threadCount = threadCount;
            _slimeAffinityRadius = slimeAffinityRadius;
            _slimeOccurenceAffinityMultiplier = slimeOccurenceAffinityMultiplier;
            _slimeTimeAffinityMultiplier = slimeTimeAffinityMultiplier;
        }

        public string Seed => _seed;
        public int TotalCountOfSimulationTicks => _totalCountOfSimulationTicks;
        public int TicksPerFrame => _ticksPerFrame;
        public int FrameRate => _frameRate;
        public int SlimeAffinityRadius => _slimeAffinityRadius;
        public int SlimeOccurenceAffinityMultiplier => _slimeOccurenceAffinityMultiplier;
        public int SlimeTimeAffinityMultiplier => _slimeTimeAffinityMultiplier;
        public int ThreadCount => _threadCount;

        public static SimulationSettings Default() {
            return new();
        }

        public static SimulationSettings SlowMotion() {
            return new(totalCountOfSimulationTicks: 50_000, ticksPerFrame: 5);
        }

        public static SimulationSettings Fast() {
            return new(totalCountOfSimulationTicks: 100_000, ticksPerFrame: 100);
        }

        public static SimulationSettings Benchmark() {
            return new SimulationSettings(totalCountOfSimulationTicks: 30_000, ticksPerFrame: 1000, frameRate: 1,
                threadCount: 1);
        }

        public static SimulationSettings QuickTest() {
            return new SimulationSettings(totalCountOfSimulationTicks: 10_000, ticksPerFrame: 10, frameRate: 60);
        }
    }
}
