using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Math;
using ShapeLib;

namespace Многоугольники
{
    static class Draw
    {
        static public void DrawPoints(Graphics g, List<Shape> ShapeList)
        {
            foreach (Shape sh in ShapeList)
                sh.Draw(g);
        }

        static public void DrawShellByDefinition(Graphics g, Pen pen, List<Shape> ShapeList, bool IsDragAndDrop, bool IsComparingEffectiveness)
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
                        if(!IsComparingEffectiveness)
                            g.DrawLine(pen, ShapeList[a].Point, ShapeList[b].Point);
                    }
                }
            }
            DeletePointsAndFinishMakingShell(ShapeList, IsDragAndDrop);
        }

        static public void DrawShellJarvis(Graphics g, Pen pen, List<Shape> ShapeList, bool IsDragAndDrop, bool IsComparingEffectiveness)
        {
            foreach (Shape sh in ShapeList)
                sh.IsInShell = false;
            Shape FirstShape, PreviousShape, ThisShape, temp;
            FirstShape = JarvisFirstShape(ShapeList);
            FirstShape.IsInShell = true;
            ThisShape = JarvisSecondShape(ShapeList, FirstShape);
            ThisShape.IsInShell = true;

            if (!IsComparingEffectiveness)
                g.DrawLine(pen, FirstShape.Point, ThisShape.Point);
            PreviousShape = FirstShape;

            do
            {
                temp = ThisShape;
                ThisShape = JarvisNextShape(ShapeList, ThisShape, PreviousShape, FirstShape);
                ThisShape.IsInShell = true;
                PreviousShape = temp;
                if (!IsComparingEffectiveness)
                    g.DrawLine(pen, ThisShape.Point, PreviousShape.Point);
            } while (ThisShape != FirstShape);

            DeletePointsAndFinishMakingShell(ShapeList, IsDragAndDrop);
        }

        static public Shape JarvisFirstShape(List<Shape> ShapeList)
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

        static public Shape JarvisSecondShape(List<Shape> ShapeList, Shape FirstShape)
        {
            Shape SecondShape = FirstShape;
            double MaxCos = double.MinValue;
            foreach (Shape sh in ShapeList)
            {
                if (!sh.IsInShell)
                {
                    if (Line.Cos(FirstShape.Point, sh.Point) > MaxCos || (Line.Cos(FirstShape.Point, sh.Point) == MaxCos && Line.Distance(FirstShape.Point, sh.Point) < Line.Distance(FirstShape.Point, SecondShape.Point)))
                    {
                        MaxCos = Line.Cos(FirstShape.Point, sh.Point);
                        SecondShape = sh;
                    }
                }
            }
            return SecondShape;
        }

        static public Shape JarvisNextShape(List<Shape> ShapeList, Shape ThisShape, Shape PreviousShape, Shape FirstShape)
        {
            Shape NextShape = PreviousShape;
            double MaxCos = double.MinValue;
            foreach (Shape sh in ShapeList)
            {
                if (!sh.IsInShell || (sh == FirstShape && sh != PreviousShape))
                {
                    if(Line.Cos(PreviousShape.Point, ThisShape.Point, sh.Point) > MaxCos || (Line.Cos(PreviousShape.Point, ThisShape.Point, sh.Point) == MaxCos && Line.Distance(PreviousShape.Point, sh.Point) < Line.Distance(PreviousShape.Point, NextShape.Point)))
                    {
                        MaxCos = Line.Cos(PreviousShape.Point, ThisShape.Point, sh.Point);
                        NextShape = sh;
                    }
                }
            }
            return NextShape;
        }

        static private void DeletePointsAndFinishMakingShell(List<Shape> ShapeList, bool IsDragAndDrop)
        {
            if (!IsDragAndDrop)
                for (int i = 0; i < ShapeList.Count; i++)
                    if (!ShapeList[i].IsInShell)
                    {
                        ShapeList.RemoveAt(i);
                        i--;
                    }
            foreach (Shape sh in ShapeList)
                sh.IsInShell = false;
        }
    }
}
