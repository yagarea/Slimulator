namespace Slimulator {
    public class SimulationSettings {
        public SimulationSettings(
            int totalCountOfSimulationTicks = 70_000,
            int ticksPerFrame = 20,
            int frameRate = 60,
            string seed = "HlenkaHelenka",
            int threadCount = 1,
            int slimeAffinityRadius = 4,
            int slimeOccurenceAffinityMultiplier = 1,
            int slimeTimeAffinityMultiplier = 10) {
            TotalCountOfSimulationTicks = totalCountOfSimulationTicks;
            TicksPerFrame = ticksPerFrame;
            FrameRate = frameRate;
            Seed = seed;
            ThreadCount = threadCount;
            SlimeAffinityRadius = slimeAffinityRadius;
            SlimeOccurenceAffinityMultiplier = slimeOccurenceAffinityMultiplier;
            SlimeTimeAffinityMultiplier = slimeTimeAffinityMultiplier;
        }

        public string Seed { get; }
        public int TotalCountOfSimulationTicks { get; }
        public int TicksPerFrame { get; }
        public int FrameRate { get; }
        public int SlimeAffinityRadius { get; }
        public int SlimeOccurenceAffinityMultiplier { get; }
        public int SlimeTimeAffinityMultiplier { get; }
        public int ThreadCount { get; }

        public static SimulationSettings Default() {
            return new SimulationSettings();
        }

        public static SimulationSettings SlowMotion() {
            return new SimulationSettings(totalCountOfSimulationTicks: 50_000, ticksPerFrame: 5);
        }

        public static SimulationSettings Fast() {
            return new SimulationSettings(totalCountOfSimulationTicks: 100_000, ticksPerFrame: 100);
        }

        public static SimulationSettings Benchmark() {
            return new SimulationSettings(totalCountOfSimulationTicks: 30_000, ticksPerFrame: 1000, frameRate: 1,
                threadCount: 1);
        }

        public static SimulationSettings QuickTest() {
            return new (totalCountOfSimulationTicks: 10_000, ticksPerFrame: 10, frameRate: 60);
        }
    }
}
