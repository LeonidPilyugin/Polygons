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
using System.Diagnostics;
using System.IO;

namespace Многоугольники
{
    public partial class Form1 : Form
    {
        enum TypeOfShell { Jarvis, ByDefinition };
        Stopwatch Watch;
        bool IsDragAndDrop;
        bool IsDragAndDropShell;
        bool IsComparingEffectiveness;
        List <Shape> ShapeList;
        List<Shape> ShapeListEffectitiveness;
        enum ShapeType {Circle, Triangle, Square};
        ShapeType ShType;
        Pen pen;
        TypeOfShell ShellType;
        Random random;
        StreamWriter stwr;
        public Form1()
        {
            ShellType = TypeOfShell.Jarvis;
            InitializeComponent();
            ShapeList = new List <Shape>();
            ShapeListEffectitiveness = new List <Shape>();
            ShapeList.Add(new Circle(Shape.Color, Shape.Radius, new Point(Width/2, Height/2)));
            IsDragAndDrop = false;
            DoubleBuffered = true;
            ShType = ShapeType.Circle;
            pen = new Pen(Color.Blue);
            random = new Random();
            Watch = new Stopwatch();
            IsComparingEffectiveness = false;
            label1.Text = label2.Text = label3.Text = "";
            panel1.Hide();
            IsDragAndDropShell = false;
        }

        private void Form1_Load(object sender, EventArgs e)  { }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (ShapeList.Count > 2)
            {
                switch (ShellType)
                {
                    case TypeOfShell.Jarvis:
                        Draw.DrawShellJarvis(e.Graphics, pen, ShapeList, IsDragAndDrop, false);
                        break;
                    case TypeOfShell.ByDefinition:
                        Draw.DrawShellByDefinition(e.Graphics, pen, ShapeList, IsDragAndDrop, false);
                        break;
                }

            }
            if (IsComparingEffectiveness)
            {
                //int n = 10;
                int m = 10;
                long[] mas1 = new long[m];
                long[] mas2 = new long[m];
                for (int n = 10; n < 2000; n+= 10)
                {
                    ShapeListEffectitiveness.Clear();
                    for (int i = 0; i < n; i++)
                        ShapeListEffectitiveness.Add(new Circle(Shape.Color, Shape.Radius, new Point(random.Next(), random.Next())));
                    Draw.DrawShellByDefinition(e.Graphics, pen, ShapeListEffectitiveness, true, true);
                    Draw.DrawShellJarvis(e.Graphics, pen, ShapeListEffectitiveness, true, true);
                    for (int j = 0; j < m; j++)
                    {
                        Watch.Restart();
                        Draw.DrawShellJarvis(e.Graphics, pen, ShapeListEffectitiveness, true, true);
                        mas1[j] = Watch.ElapsedTicks;
                        Watch.Restart();
                        Draw.DrawShellByDefinition(e.Graphics, pen, ShapeListEffectitiveness, true, true);
                        mas2[j] = Watch.ElapsedTicks;
                    }
                    /*stwr.WriteLine((double)mas1.Sum() / m + " {0:g2}", (mas1.Max() - (double)mas1.Sum() / m) * m / (double)mas1.Sum());
                    stwr.WriteLine((double)mas2.Sum() / m + " {0:g2}", (mas2.Max() - (double)mas2.Sum() / m) * m / (double)mas2.Sum());
                    stwr.WriteLine();*/
                    stwr.WriteLine("{0};{1};{2}", n, (double)mas1.Sum() / m, (double)mas2.Sum() / m);
                }
                IsComparingEffectiveness = false;
                Watch.Stop();
            }
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
                    if (true) {
                        switch (ShType)
                        {
                            case ShapeType.Circle: ShapeList.Add(new Circle(Shape.Color, Shape.Radius, e.Location)); break;
                            case ShapeType.Triangle: ShapeList.Add(new Triangle(Shape.Color, Shape.Radius, e.Location)); break;
                            case ShapeType.Square: ShapeList.Add(new Square(Shape.Color, Shape.Radius, e.Location)); break;
                        }
                    }
                    else
                        IsDragAndDropShell = true;
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
            IsDragAndDropShell = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            IsComparingEffectiveness = true;
            stwr = new StreamWriter("D:\\Учёба\\ТПП\\output.txt");
            //label1.Text = string.Format("Количество точек: {0}", n);
            ShapeListEffectitiveness.Clear();
            panel1.Show();
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = label2.Text = label3.Text = "";
            panel1.Hide();
            stwr.Close();
        }
    }
}
