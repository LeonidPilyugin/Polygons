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
    public partial class Form3 : Form
    {
        public event RadiusChangedHandler RC;
        public Form3()
        {
            InitializeComponent();
            trackBar1.Value = Shape.Radius;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            RadiusEventArgs param = new RadiusEventArgs(trackBar1.Value);
            RC(this, param);
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
