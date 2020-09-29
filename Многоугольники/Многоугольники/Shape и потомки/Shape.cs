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
        protected bool Isdd;
        protected Point delta;
        protected static Color color;
        protected static int radius;
        protected Point point;
        public abstract bool IsInside(Point p);
        public abstract void Draw(Graphics g);

        static Shape()
        {
            color = Color.Bisque;
            radius = 30;
        }
        public Shape()
        {
            point = new Point(100, 100);
            Isdd = false;
        }
        public Shape(Color color, int radius, Point point)
        {
            Shape.color = color;
            Shape.radius = radius;
            this.point = point;
            Isdd = false;
        }

        public static int Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public static Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public int X
        {
            get { return point.X; }
            set { point.X = value; }
        }
        public int Y
        {
            get { return point.Y; }
            set { point.Y = value; }
        }
        public Point Point
        {
            get { return point; }
            set { point = value; }
        }
        public bool IsDragAndDrop
        {
            get { return Isdd; }
            set { Isdd = value; }
        }
        public Point Delta
        {
            get { return delta; }
            set { delta = value; }
        }
    }
}
