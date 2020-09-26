using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using System.Drawing;

namespace Многоугольники
{
    class Triangle : Shape
    {
        public Triangle() : base() { }
        public Triangle(Color color, int radius, Point point) : base(color, radius, point) { }
        public override bool IsInside(Point p)
        {
            return p.Y <= p.X * k(points[1], points[2]) + b(points[1], points[2]) && p.Y >= p.X * k(points[0], points[1]) + b(points[0], points[1]) && p.Y >= p.X * k(points[0], points[2]) + b(points[0], points[2]);
        }

        private double k(Point a, Point b)
        {
            return (a.Y - b.Y) / (b.X - a.X);
        }
        private double b(Point a, Point b)
        {
            return a.Y - k(a, b) * a.X;
        }

        private Point[] points
        {
            get
            {
                Point[] p = new Point[3];
                p[0] = new Point(point.X, point.Y - radius);
                p[1] = new Point(point.X + (int)(radius * Sqrt(3) / 2), point.Y + radius / 2);
                p[2] = new Point(point.X - (int)(radius * Sqrt(3) / 2), point.Y + radius / 2);
                return p;
            }
        }
        public override void Draw(Graphics g)
        {
            g.FillPolygon(new SolidBrush(color), points);
        }
    }
}
