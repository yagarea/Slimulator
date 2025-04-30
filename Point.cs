namespace Slimulator {
    public enum PointType {
        Wall,
        Space,
        Slime,
        Food
    };

    public class Point {
        private int _slimeAffinity;

        public int SlimeAffinity {
            get => _slimeAffinity;
            set => _slimeAffinity = value;
        }

        public int Age { get; private set; }

        private PointType Type { get; set; }

        public int X { get; }

        public int Y { get; }


        public Point(int x, int y,PointType pt) {
            Type = pt;
            X = x;
            Y = y;
            Age = 0;
            _slimeAffinity = pt switch {
                PointType.Space => 10,
                PointType.Wall => int.MinValue,
                PointType.Food => 1_000_000,
                PointType.Slime => 0,
                _ => _slimeAffinity
            };
        }

        public Point(int x, int y, PointType pt, int lop) {
            X = x;
            Y = y;
            Age = 0;
            Type = pt;
            _slimeAffinity = lop;
        }

        public void SetType(PointType pt) {
            Type = pt;
            Age = 0;
        }
        
        public int GetSlimeAffinity() => _slimeAffinity;
        public new PointType GetType() => Type;

        public void AgeStep() {
            Age++;
        }
        
    }
}