using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Многоугольники
{
    public class RadiusEventArgs : EventArgs
    {
        public int R { get; set; }
        public RadiusEventArgs(int R)
        {
            this.R = R;
        }
    }
}
