using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void DrawPoints(Graphics g)
        {
            foreach (Shape sh in ShapeList)
                sh.Draw(g);
        }
        private void DrawShell(Graphics g, Pen pen)
        {
            bool PointsAreInShell;
            Line.Location l;
            for (int a = 0; a < ShapeList.Count - 1; a++)
            {
                for (int b = a + 1; b < ShapeList.Count; b++)
                {
                    l = Line.Location.On;
                    PointsAreInShell = true;
                    if(ShapeList[a].X == ShapeList[b].X)
                    {
                        for(int c = 0; c < ShapeList.Count; c++)
                        {
                            if (Line.location(ShapeList[c].Point, ShapeList[a].X) == Line.Location.On || c == a || c == b)
                                continue;
                            if (l == Line.Location.On)
                                l = Line.location(ShapeList[c].Point, ShapeList[a].X);
                            else
                            {
                                if(l != Line.location(ShapeList[c].Point, ShapeList[a].X))
                                {
                                    PointsAreInShell = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int c = 0; c < ShapeList.Count; c++)
                        {
                            if (c == a || c == b)
                                continue;
                            if (l == Line.Location.On)
                                l = Line.location(ShapeList[c].Point, ShapeList[a].Point, ShapeList[b].Point);
                            else
                            {
                                if (l != Line.location(ShapeList[c].Point, ShapeList[a].Point, ShapeList[b].Point))
                                {
                                    PointsAreInShell = false;
                                    break;
                                }
                            }
                        }
                    }
                    
                    if (PointsAreInShell)
                    {
                        ShapeList[a].IsInShell = ShapeList[b].IsInShell = true;
                        g.DrawLine(pen, ShapeList[a].Point, ShapeList[b].Point);
                    }
                }
            }
            if (!IsDragAndDrop)
                for (int i = 0; i < ShapeList.Count; i++)
                    if (!ShapeList[i].IsInShell)
                        ShapeList.Remove(ShapeList[i]);
            foreach (Shape sh in ShapeList)
                sh.IsInShell = false;
        }

        private void DrawShellJarvis(Graphics g)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Blue);
            if (ShapeList.Count > 2)
                DrawShell(e.Graphics, pen);
            DrawPoints(e.Graphics);
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
    }
}
