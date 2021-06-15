# ![Slimulator](https://github.com/yagarea/Slimulator/blob/master/media/logo.gif?raw=true)

Slimulator is software you can use to simulate slime mould pathfinding and enviroment exploration.
Input is image of environment and output is video of siulated slime mould behavior. 


## Input file
Input file is any standard image format supported by `FFMPEG` library. Recommended format is `PNG`.

### Legend
- **Yellow** (`FFFF00`) - Slime mould
- **Black** (`FFFFFF`) - Impenetrable wall
- **Red** (`FF0000`) - Food 
- **Every other color** - Free space

Example input:

![Example input](https://raw.githubusercontent.com/yagarea/Slimulator/master/testInputs/maze500-food2.png?raw=true)

## Output

The output of the example input above:

![Example output](https://github.com/yagarea/Slimulator/blob/master/media/exampleOutput.gif?raw=true)

## Simulation parameters

### Technical parameters

| Parameter         			| Type 	| Default | description |
--------------------------------|-------|---------|-----------------
| `totalCountOfSimulationTicks` | (int) | 70000 | Amount "ticks" (iterations of simulations) |
| `ticksPerFrame`				| (int) | 20 | Amount of ticks per one video frame. This parameter is used to control speed of simulation. Higher the number of ticks per frame higher the speed. Be aware that setting this parameter too high can result in loss of information about progress. |
| `frameRate`					| (int) | 60 |Frames per second in output video. This parameter can be used to control speed of simulation too but it is not recommended because it can damage output video quality. |
| `seed`						| (string) | "HlenkaHelenka" | Seed for random number gerenerator. Used mainly for repeatability of experiments. |
| `threadCount`					| (int) | 1 | This program can be parallelized. This parameter sets amount of threads used in |

### Slime mould behavoir parameters

| Parameter         | Type | Default |  description |
--------------------|------|---------|---------------
| `slimeAffinityRadius` 		| (int) | 4 | Euclidean distance in which have points effect on near slime mould cells. |
| `slimeOccurenceAffinityMultiplier` | (int) | 1 | Multiplier of weight of affinity of immediate advantage of point based on its surroundings. |
| `slimeTimeAffinityMultiplier` | (int) | 10 | Multiplier of weight of time on slime mould behavior (prioritizing unexplored and not recently visited places) | 

## Usage

### Wizard

This project contains basic wizard which will guide you. All you need is to run:


```bash
dotnet run Launcher.cs
```

### As framework

Here is minimal example usage of Slimulator as framework:

```c#
static void Main(string[] args) {
    Simulation sim = new Simulation(@"path/to/input/file.png",
                                    @"path/of/output/file.mp4.mp4",
                                    SimulationSettings.QuickTest());
    Console.WriteLine(sim.Start());
    sim.End(true);
}
```

In `SimulationSetting` class are following premade setups:
- **Default** - Uses default values
- **SlowMotion** - Very slow but detailed output 
- **Fast** - Very fast video ideal for large mazes
- **QuickTest** - Short video ideal for parameter tweaking
- **Benchmark** - Outputs 30 frame video with 1000 ticks per frame

You can still create your own setting by making yout own instance of `SimulationSetting` class:

```c#
SimulationSettings customSettings = new SimulationSettings(
    totalCountOfSimulationTicks: 70000,
    ticksPerFrame: 20,
    frameRate: 60,
    seed: "HlenkaHelenka",
    threadCount: 1,
    slimeAffinityRadius: 4,
    slimeOccurenceAffinityMultiplier: 1,
    slimeTimeAffinityMultiplier: 10);
```

I highly encourage you to experiment with your settings. If you discover something 
interesting feel free to contact me. I can assist you or add your discovery to this 
repository.

## Bugs and feature requests
If vitness any bugs or unwanted behavior feel free to open an issue. I am open to your 
feature requests too so do not be afraid to contact me or open issue.

## Contributions
All pull requests are welcome. Just obey current codestyle.

## Author
This software was made by [Jan Černý](https://blackblog.cz/) for 
[MatSliz Research facility](http://slimoco.ning.com/group/matsliz). If you have 
any questions about project and slime mould research visit [MatSliz official page on **Slime and mould collective**](http://slimoco.ning.com/group/matsliz) or contact me on my email address.

![MatSliz](http://storage.ning.com/topology/rest/1.0/file/get/8485133272?profile=RESIZE_180x180&crop=1%3A1&width=171)

## License
This program is published under [GPL 3](https://github.com/yagarea/Slimulator/blob/master/LICENSE)

