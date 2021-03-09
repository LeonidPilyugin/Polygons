using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ShapeLib;

namespace Многоугольники
{
    static class Shell
    {
        private static List<Shape> TempShapeList;

        static Shell()
        {
            TempShapeList = new List<Shape>();
        }

        static public void SortList(List<Shape> ShapeList)
        {
            Shape temp, PreviousShape;
            TempShapeList.Clear();
            TempShapeList.Add(Draw.JarvisFirstShape(ShapeList));
            TempShapeList.Add(Draw.JarvisSecondShape(ShapeList, TempShapeList[0]));
            PreviousShape = TempShapeList[0];
            TempShapeList[0].IsInShell = TempShapeList[1].IsInShell = true;
            do
            {
                temp = TempShapeList.Last();
                TempShapeList.Add(Draw.JarvisNextShape(ShapeList, TempShapeList.Last(), PreviousShape, TempShapeList[0]));
                TempShapeList.Last().IsInShell = true;
                PreviousShape = temp;
            } while (TempShapeList.Last() != TempShapeList[0]);
            ShapeList.Clear();
            foreach (Shape sh in TempShapeList)
                ShapeList.Add(sh);
        }

        static public bool IsInside(PointF point, List<Shape> ShapeList)
        {
            int sum = 0;
            for(int i = 0; i < ShapeList.Count; i++)
            {
                if (i == ShapeList.Count - 1)
                    sum += Line.AreCrossing(ShapeList[i].Point, ShapeList[0].Point, point) ? 1 : 0;
                else
                    sum += Line.AreCrossing(ShapeList[i].Point, ShapeList[i + 1].Point, point) ? 1 : 0;
            }
            return sum % 2 == 1;
        }
    }
}
