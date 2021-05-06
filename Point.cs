namespace Slimulator {
    public enum PointType {
        Wall,
        Space,
        Slime,
        Food
    };

    public enum PolarityType {
        Repulsive,
        Attractive,
        Unreachable
    };

    public class Point {
        private PointType _type;
        private PolarityType polarity;
        private int _slimeAffinity;

        public Point(PointType pt) {
            _type = pt;
            switch (pt) {
                case PointType.Space:
                    _slimeAffinity = 10;
                    polarity = PolarityType.Attractive;
                    break;
                case PointType.Wall:
                    _slimeAffinity = -1;
                    polarity = PolarityType.Unreachable;
                    break;
                case PointType.Food:
                    _slimeAffinity = 1000;
                    polarity = PolarityType.Attractive;
                    break;
                case PointType.Slime:
                    _slimeAffinity = 1;
                    polarity = PolarityType.Repulsive;
                    break;
            }
        }

        public Point(PointType pt, int lop) {
            _type = pt;
            _slimeAffinity = lop;
        }

        public int GetSlimeAffinity() => _slimeAffinity;
        public new PointType GetType() => _type;
        
    }
}