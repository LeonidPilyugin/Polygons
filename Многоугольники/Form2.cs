﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShapeLib;

namespace Многоугольники
{
    public partial class Form2 : Form
    {
        public event RadiusChangedHandler RC;
        public event FormClosingEventHandler CS;
        public Form2()
        {
            InitializeComponent();
            trackBar1.Value = Shape.Radius;
        }

        public void refresh()
        {
            trackBar1.Value = Shape.Radius;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            RadiusEventArgs param = new RadiusEventArgs(trackBar1.Value);
            RC(this, param);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            CS(this, e);
            e.Cancel = true;
            Hide();
        }
    }
}
