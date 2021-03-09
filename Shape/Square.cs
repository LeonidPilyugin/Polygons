using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace ShapeLib
{
    [Serializable]
    public class Square : Shape
    {
        public Square() : base() { }
        public Square(Color color, int radius, PointF point) : base(color, radius, point) { }
        public Square(PointF point) : base(point) { }

        private double Length
        {
            get { return radius * Sqrt(2); }
        }

        public override bool IsInside(Point p)
        {
            return Abs(p.X - point.X) <= Length / 2 && Abs(p.Y - point.Y) <= Length / 2;
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(brush, point.X - (int)Length / 2, point.Y - (int)Length / 2, (int)Length, (int)Length);
        }
    }
}
