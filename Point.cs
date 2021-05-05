namespace Slimulator {
    internal enum PointType {
        Wall,
        Space,
        Slime,
        Food
    };

    internal class Point {
        private PointType _type;
        private int _slimeAffinity;

        public Point(PointType pt) {
            _type = pt;
            _slimeAffinity = pt switch {
                PointType.Space => 10,
                PointType.Wall => -1,
                PointType.Food => 1000,
                PointType.Slime => 1,
                _ => _slimeAffinity
            };
        }

        public Point(PointType pt, int lop) {
            _type = pt;
            _slimeAffinity = lop;
        }

        public int GetSlimeAffinity() => _slimeAffinity;
        public new PointType GetType() => _type;
        
    }
}