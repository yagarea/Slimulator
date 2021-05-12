namespace Slimulator {
    public enum PointType {
        Wall,
        UnexploredSpace,
        ExploredSpace,
        Slime,
        Food
    }

    public class Point {
        private PointType _type;
        
        private int _slimeAffinity;
        private int _x;
        private int _y;
        private int _age;

        public PointType Type => _type;
        
        public int SlimeAffinity => _slimeAffinity;

        public int X => _x;

        public int Y => _y;
        
        public int Age => _age;

        public Point(int x, int y,PointType pt) {
            _type = pt;
            _x    = x;
            _y    = y;
            _age  = 0;
            _slimeAffinity = pt switch {
                PointType.UnexploredSpace => 10,
                PointType.Wall            => -1,
                PointType.Food            => 1000,
                PointType.Slime           => 1,
                _                         => _slimeAffinity
            };
        }

        public Point(int x, int y, PointType pt, int lop) {
            _x             = x;
            _y             = y;
            _age           = 0;
            _type          = pt;
            _slimeAffinity = lop;
        }

        public void SetType(PointType pt) {
            _type = pt;
            _age  = 0;
        }

        public int GetSlimeAffinity() => _slimeAffinity;
        
        public new PointType GetType() => _type;

        public void GetOlder() {
            _age++;
        }
    }
}