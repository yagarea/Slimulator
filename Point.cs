using System;
using System.Collections.Generic;
using System.Threading;

namespace Slimulator {
    public enum PointType {
        Wall,
        Space,
        Slime,
        Food
    };

    public class Point {
        private PointType _type;
        private int slimeAffinity;
        private int x;
        private int y;
        private int age;

        public int Age => age;

        public PointType Type => _type;

        public int SlimeAffinity => slimeAffinity;

        public int X => x;

        public int Y => y;


        public Point(int x, int y,PointType pt) {
            _type = pt;
            this.x = x;
            this.y = y;
            age = 0;
            switch (pt) {
                case PointType.Space:
                    slimeAffinity = 10;
                    break;
                case PointType.Wall:
                    slimeAffinity = int.MinValue;
                    break;
                case PointType.Food:
                    slimeAffinity = 1_000_000;
                    break;
                case PointType.Slime:
                    slimeAffinity = 0;
                    break;
            }
        }

        public Point(int x, int y, PointType pt, int lop) {
            this.x = x;
            this.y = y;
            age = 0;
            _type = pt;
            slimeAffinity = lop;
        }

        public void SetType(PointType pt) {
            this._type = pt;
            this.age = 0;
        }
        
        public int GetSlimeAffinity() => slimeAffinity;
        public new PointType GetType() => _type;

        public void GetOlder() {
            age++;
        }
        
    }
}