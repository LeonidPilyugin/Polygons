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
        Shape[] sh;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sh = new Shape[3];
            sh[0] = new Circle(Color.Cyan, 10, new Point(panel1.Width / 2, panel1.Height / 2));
            sh[1] = new Triangle(Color.Blue, 1000, new Point(panel1.Width / 2, panel1.Height / 2));
            sh[2] = new Square(Color.Blue, 100, new Point(panel1.Width / 2, panel1.Height / 2));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            sh[0].Draw(e.Graphics);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x = int.Parse(textBox1.Text);
            int y = int.Parse(textBox2.Text);
            if (sh[0].IsInside(new Point(x, y)))
                label1.Text = "принадлежит";
            else
                label1.Text = "не принадлежит";
        }
    }
}
