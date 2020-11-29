using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Math;

namespace Многоугольники
{
    static class Line
    {
        public enum Location { On, Over, Below };
        static public double k(PointF a, PointF b)
        {
            if (a.X != b.X)
                return (a.Y - b.Y) / (a.X - b.X);
            else
                return /*a.Y > b.Y ? double.MinValue :*/ double.MaxValue;
        }
        static public double b(PointF a, PointF b)
        {
            return a.Y - a.X * k(a, b);
        }
        static public Location location(PointF P, PointF A, PointF B)
        {
            if (P.Y > P.X * k(A, B) + b(A, B))
                return Location.Over;
            else if (P.Y < P.X * k(A, B) + b(A, B))
                return Location.Below;
            return Location.On;
        }
        static public Location location(PointF P, double x)
        {
            if (P.X > x)
                return Location.Over;
            else if (P.X < x)
                return Location.Below;
            return Location.On;
        }

        static public double Cos(PointF a, PointF b, PointF c)
        {
            if(a.X != b.X && b.X != c.X)
                return (k(a, b) > k(b, c) ? -1 : 1) * (1 + k(a, b) * k(b, c)) / Sqrt((1 + k(a, b)*k(a, b)) * (1 + k(b, c) * k(b, c)));
            if(b.X != c.X)
                return (k(a, b) > k(b, c) ? -1 : 1) * k(b, c) / Sqrt(1 + k(b, c) * k(b, c));
            return (k(a, b) > k(b, c) ? -1 : 1) * k(a, b) / Sqrt(1 + k(a, b) * k(a, b));
        }

        static public double Cos(PointF b, PointF c)
        {
            if(b.X != c.X)
                return (0 > k(b, c) ? -1 : 1) / Sqrt(1 + k(b, c) * k(b, c));
            return 0;
        }

        static public double Distance(PointF a, PointF b)
        {
            return Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        static public bool AreCrossing(PointF a1, PointF b1, PointF a2, PointF b2)
        {
            return location(a1, a2, b2) == location(b1, a2, b2);
        }
    }
}
