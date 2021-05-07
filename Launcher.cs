using System;
using System.Drawing.Imaging;

namespace Slimulator {
    static class Launcher {
        static void Main(string[] args) {
            String[] testFiles = new[] {"/home/john/Projects/Slimulator/test_mazes/basic-maze.png", "/home/john/Projects/Slimulator/test_mazes/small-maze-food2.png"};
            Space space = new Space(testFiles[0]);
            Simulation sim = new Simulation(space, "eeeeej", @"/home/john/Projects/Slimulator/outputvideo.mp4");
            for (int i = 0; i < 10000; i++) sim.Tick();
            sim.End(true);
        }
    }
}