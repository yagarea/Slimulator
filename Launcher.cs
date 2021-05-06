using System;
using System.Drawing.Imaging;

namespace Slimulator {
    static class Launcher {
        static void Main(string[] args) {
            Console.WriteLine($"Loading file {args[0]}");
            Space space = new Space(args[0]);
            Simulation sim = new Simulation(space, "eeej", @"/home/john/Projects/Slimulator/outputvideo.mp4");
            for(int i = 0; i < 3000; i++) sim.Tick();
            sim.End(true);
        }
    }
}