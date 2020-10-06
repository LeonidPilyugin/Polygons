using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;

namespace Многоугольники
{
    public partial class Form1 : Form
    {
        bool IsDragAndDrop;
        List <Shape> ShapeList;
        enum ShapeType {Circle, Triangle, Square};
        ShapeType ShType;
        public Form1()
        {
            InitializeComponent();
            ShapeList = new List <Shape>();
            ShapeList.Add(new Circle(Shape.Color, Shape.Radius, new Point(Width/2, Height/2)));
            IsDragAndDrop = false;
            DoubleBuffered = true;
            ShType = ShapeType.Circle;
        }

        private void Form1_Load(object sender, EventArgs e)  { }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Blue);
            if (ShapeList.Count > 2)
                Draw.DrawShell(e.Graphics, pen, ShapeList, IsDragAndDrop);
            Draw.DrawPoints(e.Graphics, ShapeList);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            IsDragAndDrop = false;
            if (e.Button == MouseButtons.Left)
            {
                foreach (Shape sh in ShapeList)
                {
                    sh.IsDragAndDrop = sh.IsInside(e.Location);
                    if (sh.IsDragAndDrop)
                    {
                        sh.Delta = new PointF(e.X - sh.Point.X, e.Y - sh.Point.Y);
                        IsDragAndDrop = true;
                    }
                }
                if (!IsDragAndDrop)
                {
                    switch (ShType)
                    {
                        case ShapeType.Circle: ShapeList.Add(new Circle(Shape.Color, Shape.Radius, e.Location)); break;
                        case ShapeType.Triangle: ShapeList.Add(new Triangle(Shape.Color, Shape.Radius, e.Location)); break;
                        case ShapeType.Square: ShapeList.Add(new Square(Shape.Color, Shape.Radius, e.Location)); break;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
                for (int i = ShapeList.Count - 1; i >= 0; i--)
                {
                    if (ShapeList[i].IsInside(e.Location))
                    {
                        ShapeList.Remove(ShapeList[i]);
                        Invalidate();
                        return;
                    }
                }
            Invalidate();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Shape sh in ShapeList)
                sh.IsDragAndDrop = false;
            IsDragAndDrop = false;
            Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragAndDrop)
            {
                foreach (Shape sh in ShapeList)
                    if (sh.IsDragAndDrop)
                    {
                        sh.X = e.X - sh.Delta.X;
                        sh.Y = e.Y - sh.Delta.Y;
                    }
                Invalidate();
            }
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShType = ShapeType.Circle;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShType = ShapeType.Triangle;
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShType = ShapeType.Square;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
    }
}
