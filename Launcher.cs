using System;

namespace Slimulator {
    static class Launcher {
        static void Main(string[] args) {
            Simulation sim = Wizard.Setup();
            Console.WriteLine(sim.Start());
            sim.End(true);
        }
    }
}
