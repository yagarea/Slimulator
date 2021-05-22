using System;

namespace Slimulator {
    static class Launcher {
        static void Main(string[] args) {
            string[] testFiles = {
                "/home/john/Projects/Slimulator/test_mazes/maze100-food2.png", 
                "/home/john/Projects/Slimulator/test_mazes/maze500-food2.png",
                "/home/john/Projects/Slimulator/test_mazes/maze1000-food2.png"
            };
            Simulation sim = new Simulation(testFiles[0],
                @$"/home/john/Projects/Slimulator/export/SlimulatorVideo-{DateTime.Now:HH-mm-ss}.mp4",
                SimulationSettings.Fast());
            Console.WriteLine(sim.Start());
            sim.End(false);
        }
    }
}