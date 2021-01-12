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
    class Circle : Shape
    {
        public Circle() : base() { }
        public Circle(Color color, int radius, PointF point) : base(color, radius, point) { }
        public Circle(PointF point) : base(point) { }

        public override bool IsInside(Point p)
        {
            return (p.X - point.X) * (p.X - point.X) + (p.Y - point.Y)* (p.Y - point.Y) <= radius * radius;
        }
        public override void Draw(Graphics g)
        {
            g.FillEllipse(brush, point.X - radius, point.Y - radius, radius * 2, radius * 2);
        }
    }
}
