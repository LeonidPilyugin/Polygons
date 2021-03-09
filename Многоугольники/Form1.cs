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
using System.Timers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ShapeLib;

namespace Многоугольники
{

    public delegate void RadiusChangedHandler(object sender, RadiusEventArgs e);
    public delegate void UnsavedWarningSolvedHandler(object sender, UnsavedWarningEventArgs e);
    public delegate void UnsavedWarningFunction();
    public partial class Form1 : Form
    {
        enum TypeOfShell { Jarvis, ByDefinition };
        Stopwatch Watch;
        bool IsDragAndDrop;
        bool IsDragAndDropShell;
        bool IsComparingEffectiveness;
        public static List <Shape> ShapeList;
        List<Shape> ShapeListEffectitiveness;
        public static ShapeType ShType;
        Pen pen;
        TypeOfShell ShellType;
        Random random;
        StreamWriter stwr;
        Form2 form2;
        Form3 form3;
        static System.Timers.Timer timer;
        ColorDialog colorDialog;
        string fileName;
        bool saved;
        Form4 unsavedWarning;
        Stack<Change> undo;
        Stack<Change> redo;
        Point delta;

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
            form2 = null;
            form3 = null;
            colorDialog = new ColorDialog();
            timer.Elapsed += OnTimedEvent;
            saved = false;
            fileName = null;
            unsavedWarning = null;
            undo = new Stack<Change>();
            redo = new Stack<Change>();
            delta = new Point();
        }

        static Form1()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 20;
            timer.AutoReset = true;
            timer.Enabled = false;
        }

        public static System.Timers.Timer Timer
        {
            get { return timer; }
        }

        void showWarning(UnsavedWarningFunction function)
        {
            Enabled = false;
            if (unsavedWarning == null)
            {
                unsavedWarning = new Form4();
                unsavedWarning.UWSC += OnUnsavedWarningResultEvent;
            }
            unsavedWarning.function = function;
            unsavedWarning.Show();
        }

        void Save(Stream fs)
        {
            BinaryFormatter bf = new BinaryFormatter();
            //FileStream fs = new FileStream(name, mode, FileAccess.Write);
            bf.Serialize(fs, ShapeList);
            bf.Serialize(fs, Shape.Color);
            bf.Serialize(fs, Shape.Radius);
            bf.Serialize(fs, pen.Color);
            fs.Close();
            saved = true;
        }

        void Back(Stream fs)
        {
            BinaryFormatter bf = new BinaryFormatter();
            //FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
            ShapeList = (List<Shape>)bf.Deserialize(fs);
            Shape.Color = (Color)bf.Deserialize(fs);
            Shape.Radius = (int)bf.Deserialize(fs);
            pen.Color = (Color)bf.Deserialize(fs);
            fs.Close();
            if (form3 != null)
                form3.refresh();
            if (form2 != null)
                form2.refresh();
        }

        private void OnUnsavedWarningResultEvent(Object sender, UnsavedWarningEventArgs e)
        {
            switch(e.result)
            {
                case Form4.Result.Cancel:
                    break;

                case Form4.Result.No:
                    if (e.function == Close)
                        saved = true;
                    e.function(); 
                    break;

                case Form4.Result.Yes:
                    saveToolStripMenuItem_Click(new Object(), new EventArgs());
                    e.function();
                    break;
            }
            Enabled = true;
        }

        private void OnTimedEvent(Object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach(Shape sh in ShapeList)
            {
                sh.X += random.Next(3) - 1;
                sh.Y += random.Next(3) - 1;
                saved = false;
            }
            Invalidate();
        }

        private void RadiusChanged(object sender, RadiusEventArgs e)
        {
            undo.Push(new ChangeRadius(e.R - Shape.Radius));
            Shape.Radius = e.R;
            Invalidate();
            saved = false;
        }
        private void TimeIntervalChanged(object sender, RadiusEventArgs e)
        {
            timer.Interval = e.R;
            saved = false;
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

        private void AddPoint(PointF point)
        {
            switch (ShType)
            {
                case ShapeType.Circle: ShapeList.Add(new Circle(Shape.Color, Shape.Radius, point)); break;
                case ShapeType.Triangle: ShapeList.Add(new Triangle(Shape.Color, Shape.Radius, point)); break;
                case ShapeType.Square: ShapeList.Add(new Square(Shape.Color, Shape.Radius, point)); break;
            }
            undo.Push(new MakePoint(ShapeList.Last()));
            saved = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            delta = e.Location;
            IsDragAndDrop = false;
            bool IsInsideAnyShape = false;
            IsDragAndDropShell = false;
            foreach (Shape sh in ShapeList)
                IsInsideAnyShape |= sh.IsInside(e.Location);
            if (e.Button == MouseButtons.Left)
            {
                foreach (Shape sh in ShapeList)
                {
                    sh.IsDragAndDrop = sh.IsInside(e.Location);
                    if (sh.IsDragAndDrop)
                    {
                        sh.DeltaX = e.X - sh.Point.X;
                        sh.DeltaY = e.Y - sh.Point.Y;
                        IsDragAndDrop = true;
                    }
                }

                if (!IsDragAndDrop)
                {
                    Shell.SortList(ShapeList);
                    if (ShapeList.Count < 3)
                        AddPoint(e.Location);
                    else
                    {
                        if (Shell.IsInside(e.Location, ShapeList))
                        {
                            IsDragAndDropShell = true;
                            foreach (Shape sh in ShapeList)
                            {
                                sh.DeltaX = e.X - sh.Point.X;
                                sh.DeltaY = e.Y - sh.Point.Y;
                            }
                        }
                        else
                            AddPoint(e.Location);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
                for (int i = ShapeList.Count - 1; i >= 0; i--)
                {
                    if (ShapeList[i].IsInside(e.Location))
                    {
                        undo.Push(new DeletePoint(ShapeList[i]));
                        ShapeList.Remove(ShapeList[i]);
                        saved = false;
                        Invalidate();
                        return;
                    }
                }
            Invalidate();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            delta.X = e.Location.X - delta.X;
            delta.Y = e.Location.Y - delta.Y;
            foreach (Shape sh in ShapeList)
            {
                if(sh.IsDragAndDrop && IsDragAndDrop)
                {
                    undo.Push(new MovePoint(sh, delta));
                }
                sh.IsDragAndDrop = false;
            }
            if(IsDragAndDropShell)
            {
                undo.Push(new MoveShell(delta));
            }
            IsDragAndDrop = false;
            IsDragAndDropShell = false;
            Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragAndDrop || IsDragAndDropShell)
            {
                foreach (Shape sh in ShapeList)
                    if (sh.IsDragAndDrop || IsDragAndDropShell)
                    {
                        sh.X = e.X - sh.Delta.X;
                        sh.Y = e.Y - sh.Delta.Y;
                    }
                saved = false;
                Invalidate();
            }
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (form2 == null)
            {
                form2 = new Form2();
                form2.RC += RadiusChanged;
                form2.Show();
            }
            else if (form2.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                form2.WindowState = FormWindowState.Normal;
            else
            {
                form2.Show();
                form2.Activate();
            }
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (form3 == null)
            {
                form3 = new Form3();
                form3.RC += TimeIntervalChanged;
                form3.Show();
            }
            else if (form3.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                form3.WindowState = FormWindowState.Normal;
            else
            {
                form3.Show();
                form3.Activate();
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            IsComparingEffectiveness = true;
            //stwr = new StreamWriter("D:\\Учёба\\ТПП\\output.txt");
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

        private void radiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void triangleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            undo.Push(new ChangeVertextype(ShType, ShapeType.Triangle));
            ShType = ShapeType.Triangle;
        }

        private void squareToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            undo.Push(new ChangeVertextype(ShType, ShapeType.Square));
            ShType = ShapeType.Square;
        }

        private void circleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            undo.Push(new ChangeVertextype(ShType, ShapeType.Circle));
            ShType = ShapeType.Circle;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void shapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            undo.Push(new ChangeColor(Shape.Color, colorDialog.Color));
            Shape.Color = colorDialog.Color;
            saved = false;
            Invalidate();
        }

        private void linesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            pen.Color = colorDialog.Color;
            saved = false;
            Invalidate();
        }

        private void newFile()
        {
            ShapeList = new List<Shape>();
            ShapeList.Add(new Circle(Shape.Color, Shape.Radius, new Point(Width / 2, Height / 2)));
            fileName = null;
            Invalidate();
            Shape.Color = Color.Bisque;
            Shape.Radius = 30;
            if (form2 != null && form2.Visible)
            {
                form2.Hide();
                form2.refresh();
            }
            if (form3 != null && form3.Visible)
            {
                form3.Hide();
                form3.refresh();
            }
        }

        private void saveFile()
        {
            saveFileDialog1.Filter = "plg files (*.plg)|*.plg|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                Save(saveFileDialog1.OpenFile());
                saved = true;
            }
        }

        private void openFile()
        {
            openFileDialog1.Filter = "plg files (*.plg)|*.plg|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                Back(openFileDialog1.OpenFile());
                saved = true;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.Filter = "plg files (*.plg)|*.plg|All files (*.*)|*.*";
            //saveFileDialog1.FilterIndex = 2;
            //saveFileDialog1.RestoreDirectory = true;
            if (!saved)
            {
                showWarning(newFile);
                return;
            }
            newFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!saved)
            {
                showWarning(openFile);
                return;
            }

            openFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName == null)
                saveFile();
            else
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write);
                Save(fs);
                saved = true;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                showWarning(saveFile);
                return;
            }
            saveFile();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                showWarning(Close);
                e.Cancel = true;
            }
        }
    }

    public class UnsavedWarningEventArgs : EventArgs
    {
        public Form4.Result result;
        public UnsavedWarningFunction function;
        public UnsavedWarningEventArgs(Form4.Result result, UnsavedWarningFunction function)
        {
            this.result = result;
            this.function += function;
        }
    }
}
