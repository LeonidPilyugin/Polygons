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
        public Form1()
        {
            InitializeComponent();
            ShapeList = new List <Shape>();
            ShapeList.Add(new Circle(Shape.Color, Shape.Radius, new Point(Width/2, Height/2)));
            IsDragAndDrop = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Shape sh in ShapeList)
                sh.Draw(e.Graphics);
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
                        sh.Delta = new Point(e.X - sh.Point.X, e.Y - sh.Point.Y);
                        IsDragAndDrop = true;
                    }
                }
                if (!IsDragAndDrop)
                {
                    ShapeList.Add(new Circle(Shape.Color, Shape.Radius, e.Location));
                    Invalidate();
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
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Shape sh in ShapeList)
                sh.IsDragAndDrop = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragAndDrop)
            {
                foreach (Shape sh in ShapeList)
                    if (sh.IsDragAndDrop)
                        sh.Point = new Point(e.X - sh.Delta.X, e.Y - sh.Delta.Y);
                Invalidate();
            }
        }
    }
}
