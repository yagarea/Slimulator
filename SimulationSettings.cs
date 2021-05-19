namespace Slimulator {
    public class SimulationSettings {
        private int _totalCountOfSimulationTicks;
        private int _ticksPerFrame;
        private int _frameRate;
        private string _seed;

        private int _slimeAffinityRadius;
        private int _slimeOccurenceAffinityMultiplier;
        private int _slimeTimeAffinityMultiplier;

        public SimulationSettings(
            int totalCountOfSimulationTicks = 70_000, 
            int ticksPerFrame = 20, int frameRate = 60,
            string seed = "HlenkaHelenka",
            int slimeAffinityRadius = 10, 
            int slimeOccurenceAffinityMultiplier = 1,
            int slimeTimeAffinityMultiplier = 1) {
            _totalCountOfSimulationTicks = totalCountOfSimulationTicks;
            _ticksPerFrame = ticksPerFrame;
            _frameRate = frameRate;
            _seed = seed;
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
        public static SimulationSettings DefaultSettings() {
            return new();
        }
        public static SimulationSettings SlowMotionSettings() {
            return new(totalCountOfSimulationTicks: 50_000, ticksPerFrame: 5);
        }
        
        public static SimulationSettings Fast() {
            return new( totalCountOfSimulationTicks:200_000, ticksPerFrame: 100);
        }
    }
}