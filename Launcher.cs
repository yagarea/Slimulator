using System;

namespace Slimulator {
    static class Launcher {
        static void Main(string[] args) {
            Console.WriteLine($"Loading file {args[0]}");
            Space space = new Space(args[0]);
            Simulation sim = new Simulation(space, "eeej", @"outputvideo.mp4");
            for(int i = 0; i < 300; i++) sim.tick();
            sim.End();
        }
    }
}