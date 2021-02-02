using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shape
{
    [Serializable]
    public abstract class Shape
    {
        [NonSerialized] protected bool Isdd;
        [NonSerialized] protected PointF delta;
        protected static Color color;
        protected static int radius;
        protected PointF point;
        [NonSerialized] protected bool IsInShell_;
        protected static SolidBrush brush;
        public abstract bool IsInside(Point p);
        public abstract void Draw(Graphics g);

        static Shape()
        {
            color = Color.Bisque;
            radius = 30;
            brush = new SolidBrush(color);
        }
        public Shape()
        {
            point = new PointF(100, 100);
            Isdd = false;
            IsInShell_ = false;
        }
        public Shape(Color color, int radius, PointF point)
        {
            Shape.color = color;
            brush = new SolidBrush(color);
            Shape.radius = radius;
            this.point = point;
            Isdd = false;
        }

        public Shape(PointF point)
        {
            this.point = point;
        }

        public static int Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public static Color Color
        {
            get { return color; }
            set { color = value; brush = new SolidBrush(color); }
        }

        public float X
        {
            get { return point.X; }
            set { point.X = value; }
        }
        public float Y
        {
            get { return point.Y; }
            set { point.Y = value; }
        }
        public PointF Point
        {
            get { return point; }
            set { point = value; }
        }
        public bool IsDragAndDrop
        {
            get { return Isdd; }
            set { Isdd = value; }
        }
        public PointF Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        public float DeltaX
        {
            get { return delta.X; }
            set { delta.X = value; }
        }

        public float DeltaY
        {
            get { return delta.Y; }
            set { delta.Y = value; }
        }

        public bool IsInShell
        {
            get { return IsInShell_; }
            set { IsInShell_ = value; }
        }
    }
}
