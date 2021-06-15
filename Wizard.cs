using System;

namespace Slimulator {
    public class Wizard {
        public static Simulation Setup() {
            PrintLogo();
            Console.Write("Path of input file: ");
            String input = Console.ReadLine();
            String output = $@"/home/john/Projects/Slimulator/export/SlimulatorVideo-{DateTime.Now:HH-mm-ss}.mp4";
            Console.Write($"Path of input file (default: {output}): ");
            String customOutput = Console.ReadLine();
            if (!string.IsNullOrEmpty(customOutput)) output = customOutput;
            Console.Write($@"[0] Default    - Uses default values
[1] SlowMotion  - Very slow but detailed output
[2] Fast        - Very fast video ideal for large mazes
[3] QuickTest   - Short video ideal for parameter tweaking
[4] Benchmark   - Outputs 30 frame video with 1000 ticks per frame

Choice (default 0): ");
            String choiceOfSettings = Console.ReadLine().Trim();
            int cos = 0;
            if (!String.IsNullOrEmpty(choiceOfSettings)) cos = int.Parse(choiceOfSettings);
            SimulationSettings ss = SimulationSettings.Default();
            switch (cos) {
                case 0:
                    break;
                case 1:
                    ss = SimulationSettings.SlowMotion();
                    break;
                case 2: ss = SimulationSettings.Fast();
                    break;
                case 3: ss = SimulationSettings.QuickTest();
                    break;
                case 4: ss = SimulationSettings.Benchmark();
                    break;
                default:
                    Console.WriteLine("Invalid settings");
                    Environment.Exit(1);
                    break;
            }

            Console.WriteLine(input);
            return new Simulation(input, output, ss);
        }

        private static void PrintLogo() {
            Console.Write(@"                                                                                         
  _____ _ _                 _       _             
 / ____| (_)               | |     | |            
| (___ | |_ _ __ ___  _   _| | __ _| |_ ___  _ __ 
 \___ \| | | '_ ` _ \| | | | |/ _` | __/ _ \| '__|
 ____) | | | | | | | | |_| | | (_| | || (_) | |   
|_____/|_|_|_| |_| |_|\__,_|_|\__,_|\__\___/|_|   
");
        }
    }
}