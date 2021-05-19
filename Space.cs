using System;
using System.Collections.Generic;
using System.Drawing;

namespace Slimulator {
    public class Space {
        private readonly int _height;

        public int Height => _height;

        public int Width => _width;

        private readonly int _width;
        private readonly Point[,] _points;

        private PointType TypeOfColor(Color c) {
            return c.R switch {
                0 when c.G == 0 && c.B == 0 => PointType.Wall,
                255 when c.G == 0 && c.B == 0 => PointType.Food,
                255 when c.G == 255 && c.B == 0 => PointType.Slime,
                _ => PointType.Space
            };
        }

        private Color ColorOfType(PointType pt) {
            return pt switch {
                PointType.Wall => Color.FromKnownColor(KnownColor.Black),
                PointType.Slime => Color.FromKnownColor(KnownColor.Yellow),
                PointType.Food => Color.FromKnownColor(KnownColor.Red),
                _ => Color.FromKnownColor(KnownColor.White)
            };
        }

        public Space(string inputPath) {
            Console.WriteLine($"Loading file {inputPath}");
            Bitmap bm = new Bitmap(inputPath);
            _height = bm.Height;
            _width = bm.Width;
            _points = new Point[_width, _height];
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) _points[x, y] = new Point(x, y, TypeOfColor(bm.GetPixel(x, y)));
            }

            Console.WriteLine($"Space constructed [{_height}x{_width}]");
        }

        public Bitmap ExportBitmap() {
            Bitmap outbm = new Bitmap(_height, _width);
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) outbm.SetPixel(x, y, ColorOfType(_points[x, y].GetType()));
            }

            return outbm;
        }

        private bool IsInBound(int x, int y) {
            return x < 0 || x >= _width || y < 0 || y >= _height;
        }

        public Point GetPoint(int x, int y) {
            if (IsInBound(x, y)) return new Point(x, y, PointType.Wall);
            return _points[x, y];
        }

        public PointType GetPointType(int x, int y) {
            if (IsInBound(x, y)) return PointType.Wall;
            return _points[x, y].GetType();
        }

        public HashSet<Point> GetAccessibleNeighbours(int x, int y) {
            HashSet<Point> neighbours = new HashSet<Point>();
            for (int shiftX = -1; shiftX <= 1; shiftX++) {
                for (int shiftY = -1; shiftY <= 1; shiftY++) {
                    if (GetPointType(x + shiftX, y + shiftY) != PointType.Wall &&
                        GetPointType(x + shiftX, y + shiftY) != PointType.Slime) {
                        neighbours.Add(GetPoint(x + shiftX, y + shiftY));
                    }
                }
            }

            return neighbours;
        }

        public bool IsInMiddleOfSlime(int x, int y) {
            foreach (int shift in new[] {-1, 1}) {
                if (GetPointType(x + shift, y) != PointType.Slime ||
                    GetPointType(x, y + shift) != PointType.Slime) {
                    return false;
                }
            }

            return true;
        }

        public void GetOlder() {
            for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
                _points[x, y].GetOlder();
        }

        public void TextLog() {
            Console.Write('\n');
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    switch (_points[x, y].GetType()) {
                        case PointType.Wall:
                            Console.Write("#");
                            break;
                        case PointType.Slime:
                            Console.Write('*');
                            break;
                        case PointType.Food:
                            Console.Write('@');
                            break;
                        default:
                            Console.Write('.');
                            break;
                    }

                    Console.Write(' ');
                }

                Console.Write('\n');
            }
        }

        private double DistanceOf(Point a, Point b) {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public int OccurrenceInRadius(Point p, PointType searchedPointType, int radius) {
            int occurance = 0;
            radius = Math.Abs(radius);
            for (int xShift = -radius; xShift <= radius; xShift++) {
                for (int yShift = -radius; yShift <= radius; yShift++) {
                    if (DistanceOf(p, GetPoint(p.X + xShift, p.Y + yShift)) <= radius &&
                        GetPointType(p.X + xShift, p.Y + yShift) == searchedPointType) occurance++;
                }
            }
            return occurance;
        }

        public void AffinityLog() {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) Console.Write(_points[x, y].GetSlimeAffinity() + " ");
                Console.Write('\n');
            }
        }
    }
}