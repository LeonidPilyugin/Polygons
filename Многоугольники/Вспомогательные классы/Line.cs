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
            return (a.Y - b.Y) / (a.X - b.X);
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
            return -(1 + k(a, b) * k(b, c)) / Sqrt((1 + k(a, b)*k(a, b)) * (1 + k(b, c) * k(b, c)));
        }
    }
}
