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
    public partial class Form4 : Form
    {
        public enum Result{Yes, No, Cancel};
        public event UnsavedWarningSolvedHandler UWSC;
        public UnsavedWarningFunction function;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UnsavedWarningEventArgs param = new UnsavedWarningEventArgs(Result.Yes, function);
            UWSC(this, param);
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnsavedWarningEventArgs param = new UnsavedWarningEventArgs(Result.No, function);
            UWSC(this, param);
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UnsavedWarningEventArgs param = new UnsavedWarningEventArgs(Result.Cancel, function);
            UWSC(this, param);
            Hide();
        }
    }
}
