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
            Shape FirstShape, PreviousShape, ThisShape, temp;

            FirstShape = JarvisFirstShape(ShapeList);
            ThisShape = JarvisSecondShape(ShapeList, FirstShape);

            g.DrawLine(pen, FirstShape.Point, ThisShape.Point);
            PreviousShape = FirstShape;
            ThisShape.IsInShell = PreviousShape.IsInShell = true;

            do
            {
                temp = ThisShape;
                ThisShape = JarvisNextShape(ShapeList, ThisShape, PreviousShape, FirstShape);
                ThisShape.IsInShell = true;
                PreviousShape = temp;
                g.DrawLine(pen, ThisShape.Point, PreviousShape.Point);
            } while (ThisShape != FirstShape);

            DeletePointsAndFinishMakingShell(ShapeList, IsDragAndDrop);
        }

        static private Shape JarvisFirstShape(List<Shape> ShapeList)
        {
            Shape FirstShape = ShapeList[0];
            foreach (Shape sh in ShapeList)
            {
                if (sh.Y < FirstShape.Y)
                    FirstShape = sh;
                else if (sh.Y == FirstShape.Y && sh.X < FirstShape.X)
                    FirstShape = sh;
            }
            return FirstShape;
        }

        static private Shape JarvisSecondShape(List<Shape> ShapeList, Shape FirstShape)
        {
            Shape SecondShape = FirstShape;
            double a = double.MaxValue, a1;
            foreach (Shape sh in ShapeList)
            {
                if (!sh.IsInShell)
                {
                    a1 = Atan(Line.k(sh.Point, FirstShape.Point));
                    if (a1 < 0)
                        a1 += PI;
                    if (a1 < a)
                    {
                        SecondShape = sh;
                        a = a1;
                    }
                }
            }
            return SecondShape;
        }

        static private Shape JarvisNextShape(List<Shape> ShapeList, Shape ThisShape, Shape PreviousShape, Shape FirstShape)
        {
            Shape TempShape = PreviousShape;
            double MinCos = double.MaxValue;
            foreach (Shape sh in ShapeList)
            {
                if (!sh.IsInShell || (sh == FirstShape && PreviousShape != FirstShape))
                {
                    if(Line.Cos(PreviousShape.Point, ThisShape.Point, sh.Point) < MinCos)
                    {
                        MinCos = Line.Cos(PreviousShape.Point, ThisShape.Point, sh.Point);
                        TempShape = sh;
                    }
                }
            }
            return TempShape;
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
