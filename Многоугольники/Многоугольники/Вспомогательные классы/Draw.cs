using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Math;

namespace Многоугольники
{
    static class Draw
    {
        static public void DrawPoints(Graphics g, List<Shape> ShapeList)
        {
            foreach (Shape sh in ShapeList)
                sh.Draw(g);
        }

        static public void DrawShell(Graphics g, Pen pen, List<Shape> ShapeList, bool IsDragAndDrop)
        {
            bool PointsAreInShell;
            Line.Location l;
            for (int a = 0; a < ShapeList.Count - 1; a++)
            {
                for (int b = a + 1; b < ShapeList.Count; b++)
                {
                    l = Line.Location.On;
                    PointsAreInShell = true;
                    if (ShapeList[a].X == ShapeList[b].X)
                    {
                        for (int c = 0; c < ShapeList.Count; c++)
                        {
                            if (Line.location(ShapeList[c].Point, ShapeList[a].X) == Line.Location.On || c == a || c == b)
                                continue;
                            if (l == Line.Location.On)
                                l = Line.location(ShapeList[c].Point, ShapeList[a].X);
                            else
                            {
                                if (l != Line.location(ShapeList[c].Point, ShapeList[a].X))
                                {
                                    PointsAreInShell = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int c = 0; c < ShapeList.Count; c++)
                        {
                            if (c == a || c == b)
                                continue;
                            if (l == Line.Location.On)
                                l = Line.location(ShapeList[c].Point, ShapeList[a].Point, ShapeList[b].Point);
                            else
                            {
                                if (l != Line.location(ShapeList[c].Point, ShapeList[a].Point, ShapeList[b].Point))
                                {
                                    PointsAreInShell = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (PointsAreInShell)
                    {
                        ShapeList[a].IsInShell = ShapeList[b].IsInShell = true;
                        g.DrawLine(pen, ShapeList[a].Point, ShapeList[b].Point);
                    }
                }
            }
            DeletePointsAndFinishMakingShell(ShapeList, IsDragAndDrop);
        }

        static public void DrawShellJarvis(Graphics g, Pen pen, List<Shape> ShapeList, bool IsDragAndDrop)
        {
            Shape FirstShape = ShapeList[0], PreviousShape, NextShape, ThisShape;
            double a, a1, a2;
            foreach (Shape sh in ShapeList)
            {
                if (sh.Y < FirstShape.Y)
                    FirstShape = sh;
                else if (sh.Y == FirstShape.Y && sh.X < FirstShape.X)
                    FirstShape = sh;
            }
            FirstShape.IsInShell = true;
            ThisShape = FirstShape;
            a = Double.MaxValue;
            foreach (Shape sh in ShapeList)
            {
                a1 = Atan(Line.k(sh.Point, FirstShape.Point));
                if (a1 < 0)
                    a1 += PI;
                if (a1 < a)
                {
                    ThisShape = sh;
                    a = a1;
                }
            }
            ThisShape.IsInShell = true;
            g.DrawLine(pen, FirstShape.Point, ThisShape.Point);

            NextShape = PreviousShape = FirstShape;
            do
            {
                a = Double.MaxValue;
                a1 = Atan(Line.k(ThisShape.Point, PreviousShape.Point));
                if (a1 < 0)
                    a1 += PI;
                foreach (Shape sh in ShapeList)
                {
                    if (!sh.IsInShell || sh == FirstShape)
                    {
                        a2 = Atan(Line.k(ThisShape.Point, sh.Point));
                        if (a2 < 0)
                            a2 += PI;
                        if (a2 - a1 < a)
                        {
                            a = a2 - a1;
                            NextShape = sh;
                        }
                    }
                }
                PreviousShape = ThisShape;
                ThisShape = NextShape;
                ThisShape.IsInShell = true;
                g.DrawLine(pen, PreviousShape.Point, ThisShape.Point);
            } while (ThisShape != FirstShape);

            DeletePointsAndFinishMakingShell(ShapeList, IsDragAndDrop);
        }

        static private void DeletePointsAndFinishMakingShell(List<Shape> ShapeList, bool IsDragAndDrop)
        {
            if (!IsDragAndDrop)
                for (int i = 0; i < ShapeList.Count; i++)
                    if (!ShapeList[i].IsInShell)
                        ShapeList.Remove(ShapeList[i]);
            foreach (Shape sh in ShapeList)
                sh.IsInShell = false;
        }
    }
}
