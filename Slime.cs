using System;
using System.Collections.Generic;

namespace Slimulator {
    public class Slime {
        public static HashSet<Point> FindAllSlime(Space s) {
            HashSet<Point> outputList = new HashSet<Point>();
            for (int x = 0; x < s.Width; x++) {
                for (int y = 0; y < s.Height; y++) {
                    if (s.GetPointType(x, y) == PointType.Slime) outputList.Add(s.GetPoint(x, y));
                }
            }

            return outputList;
        }

        public static HashSet<Point> FindAllPossiblePlacesToMove(Space s, HashSet<Point> slime) {
            HashSet<Point> output = new HashSet<Point>();
            foreach (Point p in slime) {
                foreach (Point n in s.GetAccessibleNeighbours(p.X, p.Y)) {
                    output.Add(n);
                }
            }

            return output;
        }
    }
}