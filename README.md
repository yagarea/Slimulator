# ![Slimulator](https://github.com/yagarea/Slimulator/blob/master/media/logo.gif?raw=true)

Slimulator is software you can use to simulate slime mold pathfinding and enviroment exploration.
Input is image of environment and output is video of siulated slime mold behavior. 


## Input file
Input file is any standard image format supported by `FFMPEG` library. Recommended format is `PNG`.

### Legend
- **Yellow** (`FFFF00`) - Slime mold
- **Black** (`FFFFFF`) - Impenetrable wall
- **Red** (`FF0000`) - Food 
- **Every other color** - Free space

Example input:

![Example input](https://raw.githubusercontent.com/yagarea/Slimulator/master/testInputs/maze500-food2.png?raw=true)

## Simulation parameters

### Technical parameters

| Parameter         			| Type 	| Default | description |
--------------------------------|-------|---------|-----------------
| `totalCountOfSimulationTicks` | (int) | 70000 | Amount "ticks" (iterations of simulations) |
| `ticksPerFrame`				| (int) | 20 | Amount of ticks per one video frame. This parameter is used to control speed of simulation. Higher the number of ticks per frame higher the speed. Be aware that setting this parameter too high can result in loss of information about progress. |
| `frameRate`					| (int) | 60 |Frames per second in output video. This parameter can be used to control speed of simulation too but it is not recommended because it can damage output video quality. |
| `seed`						| (string) | "HlenkaHelenka" | Seed for random number gerenerator. Used mainly for repeatability of experiments. |
| `threadCount`					| (int) | 1 | This program can be parallelized. This parameter sets amount of threads used in |

### Slime mold behavoir parameters

| Parameter         | Type | Default |  description |
--------------------|------|---------|---------------
| `slimeAffinityRadius` 		| (int) | 4 | Euclidean distance in which have points effect on near slime mold cells. |
| `slimeOccurenceAffinityMultiplier` | (int) | 1 | Multiplier of weight of affinity of immediate advantage of point based on its surroundings. |
| `slimeTimeAffinityMultiplier` | (int) | 10 | Multiplier of weight of time on slime mold behavior (prioritizing unexplored and not recently visited places) | 

# Author
This software was made by [Jan Černý](https://blackblog.cz/) for (MatSliz Research facility)[http://slimoco.ning.com/group/matsliz]. If you have any questions about project and slime mold research visit [] or contact me on my email address.

## Contributions
All pull requests are welcome.

## License
This program is published under [GPL 3](https://github.com/yagarea/Slimulator/blob/master/LICENSE)

