using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Math;

namespace Многоугольники
{
    abstract class Shape
    {
        protected static Color color;
        protected static int radius;
        protected Point point;
        public abstract bool IsInside(Point p);
        public abstract void Draw(Graphics g);

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public Point Point
        {
            get { return point; }
            set { point = value; }
        }
    }

    class Circle:Shape{
        public Circle()
        {
            color = Color.AliceBlue;
            radius = 10;
            point = new Point(100, 100);
        }
        public Circle(Color color, int radius, Point point)
        {
            Shape.color = color;
            Shape.radius = radius;
            this.point = point;
        }

        private Rectangle rectangle
        {
            get { return new Rectangle(point.X - radius, point.Y - radius, radius * 2, radius * 2); }
        }

        public override bool IsInside(Point p)
        {
            if (Pow(p.X - point.X, 2) + Pow(p.Y - point.Y, 2) <= Pow(radius, 2))
                return true;
            return false;
        }
        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(color), rectangle);
        }
    }

    class Triangle : Shape
    {
        public Triangle()
        {
            color = Color.AliceBlue;
            radius = 10;
            point = new Point(100, 100);
        }
        public Triangle(Color color, int radius, Point point)
        {
            Shape.color = color;
            Shape.radius = radius;
            this.point = point;
        }
        public override bool IsInside(Point p)
        {
            if ((points[0].X - p.X) * (points[1].Y - points[0].Y) <= (points[1].X - points[0].X) * (points[0].Y - p.Y) && (points[1].X - p.X) * (points[2].Y - points[1].Y) <= (points[2].X - points[1].X) * (points[1].Y - p.Y) && (points[2].X - p.X) * (points[0].Y - points[2].Y) <= (points[0].X - points[2].X) * (points[2].Y - p.Y))
                return true;
            if ((points[0].X - p.X) * (points[1].Y - points[0].Y) >= (points[1].X - points[0].X) * (points[0].Y - p.Y) && (points[1].X - p.X) * (points[2].Y - points[1].Y) >= (points[2].X - points[1].X) * (points[1].Y - p.Y) && (points[2].X - p.X) * (points[0].Y - points[2].Y) >= (points[0].X - points[2].X) * (points[2].Y - p.Y))
                return true;
            return false;
        }
        private Point[] points {
            get
            {
                Point[] p = new Point[3];
                p[0] = new Point(point.X, point.Y - radius);
                p[1] = new Point(point.X + (int)(radius * Pow(3, 0.5)/2), point.Y + radius / 2);
                p[2] = new Point(point.X - (int)(radius * Pow(3, 0.5)/2), point.Y + radius / 2);
                return p;
            }
        }
        public override void Draw(Graphics g)
        {
            g.FillPolygon(new SolidBrush(color), points);
        }
    }

    class Square : Shape
    {
        public Square()
        {
            color = Color.AliceBlue;
            radius = 10;
            point = new Point(100, 100);
        }
        public Square(Color color, int radius, Point point)
        {
            Shape.color = color;
            Shape.radius = radius;
            this.point = point;
        }

        private Rectangle rectangle
        {
            get { return new Rectangle(point.X - (int)Length / 2, point.Y - (int)Length / 2, (int)Length, (int)Length); }
        }

        private double Length{
            get { return 2 * (radius / Pow(2, 0.5)); }
        }

        public override bool IsInside(Point p)
        {
            if (Abs(p.X - point.X) <= Length / 2 && Abs(p.Y - point.Y) <= Length / 2)
                return true;
            return false;
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), rectangle);
        }
    }
}
