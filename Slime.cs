using System.Collections.Generic;

namespace Slimulator {
    public class Slime {
        public static HashSet<Point> FindAllSlime(Space s) {
            HashSet<Point> outputList = new HashSet<Point>();
            for (int x = 0; x < s.Width; x++)
                for (int y = 0; y < s.Height; y++)
                    if (s.GetPointType(x, y) == PointType.Slime)
                        outputList.Add(s.GetPoint(x, y));

            return outputList;
        }

        public static HashSet<Point> FindAllPossiblePlacesToMove(Space s, HashSet<Point> slime) {
            HashSet<Point> output = new HashSet<Point>();
            foreach (Point p in slime)
                foreach (Point n in s.GetAccessibleNeighbours(p.X, p.Y))
                    output.Add(n);

            return output;
        }
        
        public static HashSet<Point> FindAllPossibleSlimesToPerish(Space s, HashSet<Point> slime) {
            HashSet<Point> borderSlime = new HashSet<Point>();
            foreach (Point p in slime)
                if (!s.IsInMiddleOfSlime(p.X, p.Y))
                    borderSlime.Add(p);

            int highestAge = -1;
            HashSet<Point> output = new HashSet<Point>();
            foreach (Point p in borderSlime)
                if (p.Age > highestAge) {
                    output.Clear();
                    output.Add(p);
                    highestAge = p.Age;
                } else if (p.Age == highestAge)
                    output.Add(p);

            return output;
        }
    }
}