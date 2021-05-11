using System.Collections.Generic;
using System.Threading;

namespace Slimulator {
    public enum PointType {
        Wall,
        UnexploredSpace,
        ExploredSpace,
        Slime,
        Food
    };

    public class Point {
        private PointType _type;
        private int _slimeAffinity;
        private int x;
        private int y;
        private int age;

        public int Age => age;

        public PointType Type => _type;

        public int SlimeAffinity => _slimeAffinity;

        public int X => x;

        public int Y => y;


        public Point(int x, int y,PointType pt) {
            _type = pt;
            this.x = x;
            this.y = y;
            age = 0;
            switch (pt) {
                case PointType.UnexploredSpace:
                    _slimeAffinity = 10;
                    break;
                case PointType.Wall:
                    _slimeAffinity = -1;
                    break;
                case PointType.Food:
                    _slimeAffinity = 1000;
                    break;
                case PointType.Slime:
                    _slimeAffinity = 1;
                    break;
            }
        }

        public Point(int x, int y, PointType pt, int lop) {
            this.x = x;
            this.y = y;
            age = 0;
            _type = pt;
            _slimeAffinity = lop;
        }

        public void SetType(PointType pt) {
            this._type = pt;
            this.age = 0;
        }
        
        public int GetSlimeAffinity() => _slimeAffinity;
        public new PointType GetType() => _type;

        public void GetOlder() {
            age++;
        }
        
    }
}