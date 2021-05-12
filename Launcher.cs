namespace Slimulator {
    static class Launcher {
        private static void Main(string[] args) {
            string[] testFiles = {
                "/home/john/Projects/Slimulator/test_mazes/maze100-food2.png",
                "/home/john/Projects/Slimulator/test_mazes/maze500-food2.png",
                "/home/john/Projects/Slimulator/test_mazes/maze1000-food2.png"
            };
            Space space = new Space(testFiles[1]);
            Simulation sim = new Simulation(space, 
                "/home/john/Projects/Slimulator/outputvideo.mp4",
                50000,
                ticksPerFrame: 20, frameRate:60);
            sim.Start();
            sim.End(true);
        }
    }
}