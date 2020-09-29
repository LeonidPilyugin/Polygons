using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Многоугольники
{
    static class Line
    {
        public enum Location { On, Over, Below};
        static public double k(Point a, Point b)
        {
            return (a.Y - b.Y) / (b.X - a.X);
        }
        static private double b(Point a, Point b)
        {
            return a.Y - k(a, b) * a.X;
        }
        static public Location location(Point P, double k, double b)
        {
            if (P.Y > P.X * k + b)
                return Location.Over;
            else if (P.Y < P.X * k + b)
                return Location.Below;
            return Location.On;
        }
        static public Location location(Point P, double x)
        {
            if (P.X > x)
                return Location.Over;
            else if (P.X < x)
                return Location.Below;
            return Location.On;
        }
    }
}
