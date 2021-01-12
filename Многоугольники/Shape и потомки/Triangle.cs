using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using System.Drawing;

namespace Многоугольники
{
    [Serializable]
    class Triangle : Shape
    {
        public Triangle() : base() { }
        public Triangle(Color color, int radius, PointF point) : base(color, radius, point) { }
        public Triangle(PointF point) : base(point) { }

        public override bool IsInside(Point p)
        {
            return p.Y <= p.X * k(points[1], points[2]) + b(points[1], points[2]) && p.Y >= p.X * k(points[0], points[1]) + b(points[0], points[1]) && p.Y >= p.X * k(points[0], points[2]) + b(points[0], points[2]);
        }

        private double k(PointF a, PointF b)
        {
            return (a.Y - b.Y) / (b.X - a.X);
        }
        private double b(PointF a, PointF b)
        {
            return a.Y - k(a, b) * a.X;
        }

        private PointF[] points
        {
            get
            {
                PointF[] p = new PointF[3];
                p[0] = new PointF(point.X, point.Y - radius);
                p[1] = new PointF(point.X + (int)(radius * Sqrt(3) / 2), point.Y + radius / 2);
                p[2] = new PointF(point.X - (int)(radius * Sqrt(3) / 2), point.Y + radius / 2);
                return p;
            }
        }
        public override void Draw(Graphics g)
        {
            g.FillPolygon(brush, points);
        }
    }
}
