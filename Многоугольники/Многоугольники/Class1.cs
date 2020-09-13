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
        public abstract bool IsClicked(Point p);
        public abstract void Draw(Graphics g);
    }

    class Circle:Shape{
        private Rectangle rectangle;
        public Circle()
        {
            color = Color.AliceBlue;
            radius = 10;
            point = new Point(100, 100);
            rectangle = new Rectangle(point.X - radius, point.Y - radius, radius*2, radius*2);
        }
        public Circle(Color color, int radius, Point point)
        {
            Shape.color = color;
            Shape.radius = radius;
            this.point = point;
            rectangle = new Rectangle(point.X - radius, point.Y - radius, radius * 2, radius * 2);
        }
        public override bool IsClicked(Point p)
        {
            if (Pow(p.X - point.X, 2) + Pow(p.Y - point.Y, 2) <= Pow(radius, 2))
                return true;
            return false;
        }
        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Shape.color), rectangle);
        }
    }

    class Triangle : Shape
    {
        private Point[] points;
        public Triangle()
        {
            color = Color.AliceBlue;
            radius = 10;
            point = new Point(100, 100);
            points = new Point[3];
            points[0] = new Point(point.X, point.Y + radius);
            points[1] = new Point(point.X + (int)(radius * Pow(3, 0.5)), point.Y - radius / 2);
            points[2] = new Point(point.X - (int)(radius * Pow(3, 0.5)), point.Y - radius / 2);
        }
        public Triangle(Color color, int radius, Point point)
        {
            Shape.color = color;
            Shape.radius = radius;
            this.point = point;
            points = new Point[3];
            points[0] = new Point(point.X, point.Y + radius);
            points[1] = new Point(point.X + (int)(radius * Pow(3, 0.5)), point.Y - radius / 2);
            points[2] = new Point(point.X - (int)(radius * Pow(3, 0.5)), point.Y - radius / 2);
        }
        public Triangle(Color color, Point point, Point[] points)
        {
            Shape.color = color;
            Shape.radius = Radius(points[0], points[1], points[2]);
            this.point = point;
            this.points = points;
        }
        public override bool IsClicked(Point p)
        {
            if ((points[0].X - p.X) * (points[1].Y - points[0].Y) <= (points[1].X - points[0].X) * (points[0].Y - p.Y) && (points[1].X - p.X) * (points[2].Y - points[1].Y) <= (points[2].X - points[1].X) * (points[1].Y - p.Y) && (points[2].X - p.X) * (points[0].Y - points[2].Y) <= (points[0].X - points[2].X) * (points[2].Y - p.Y))
                return true;
            if ((points[0].X - p.X) * (points[1].Y - points[0].Y) >= (points[1].X - points[0].X) * (points[0].Y - p.Y) && (points[1].X - p.X) * (points[2].Y - points[1].Y) >= (points[2].X - points[1].X) * (points[1].Y - p.Y) && (points[2].X - p.X) * (points[0].Y - points[2].Y) >= (points[0].X - points[2].X) * (points[2].Y - p.Y))
                return true;
            return false;
        }
        private int Radius(Point A, Point B, Point C)
        {
            double a = distance(B, C);
            double b = distance(A, C);
            double c = distance(B, A);
            return (int)(a*b*c/4/Area(a, b, c));
        }
        private double Area(double a, double b, double c) {
            double p = (a + b + c) / 2;
            return Pow(p * (p - a) * (p - b) * (p - c), 2);
        }
        private double distance(Point A, Point B) {
            return Pow(Pow(B.X - A.X, 2) + Pow(B.Y - A.Y, 2), 2);
        }
        public override void Draw(Graphics g)
        {
            g.FillPolygon(new SolidBrush(Shape.color), points);
        }
    }

    class Square : Shape
    {
        private Rectangle rectangle;
        public Square()
        {
            color = Color.AliceBlue;
            radius = 10;
            point = new Point(100, 100);
            rectangle = new Rectangle(point.X - (int)Length/2, point.Y - (int)Length / 2, (int)Length, (int)Length);
        }
        public Square(Color color, int radius, Point point)
        {
            Shape.color = color;
            Shape.radius = radius;
            this.point = point;
            rectangle = new Rectangle(point.X - (int)Length / 2, point.Y - (int)Length / 2, (int)Length, (int)Length);
        }

        public double Length{
            get { return 2 * (radius / Pow(2, 0.5)); }
        }

        public override bool IsClicked(Point p)
        {
            if (Abs(p.X - point.X) <= Length / 2 && Abs(p.Y - point.Y) <= Length / 2)
                return true;
            return false;
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Shape.color), rectangle);
        }
    }
}
