using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ShapeLib;

namespace Многоугольники
{
    abstract class Change
    {
        public abstract void Redo();
        public abstract void Undo();
    }

    class MovePoint : Change
    {
        private Shape shape;
        private Point delta;
        public MovePoint(Shape shape, Point delta)
        {
            this.shape = shape;
            this.delta = delta;
        }

        public override void Redo()
        {
            shape.X += delta.X;
            shape.Y += delta.Y;
        }
        public override void Undo()
        {
            shape.X -= delta.X;
            shape.Y -= delta.Y;
        }
    }

    class MoveShell : Change
    {
        private Point delta;

        public MoveShell(Point delta)
        {
            this.delta = delta;
        }

        public override void Redo()
        {
            foreach(Shape shape in Form1.ShapeList)
            {
                shape.X += delta.X;
                shape.Y += delta.Y;
            }
        }
        public override void Undo()
        {
            foreach (Shape shape in Form1.ShapeList)
            {
                shape.X -= delta.X;
                shape.Y -= delta.Y;
            }
        }
    }

    class MakePoint : Change
    {
        private Shape shape;

        public MakePoint(Shape shape)
        {
            this.shape = shape;
        }

        public override void Redo()
        {
            Form1.ShapeList.Add(shape);
        }
        public override void Undo()
        {
            Form1.ShapeList.Remove(shape);
        }
    }

    class DeletePoint : Change
    {
        private Shape shape;

        public DeletePoint(Shape shape)
        {
            this.shape = shape;
        }

        public override void Redo()
        {
            Form1.ShapeList.Remove(shape);
        }
        public override void Undo()
        {
            Form1.ShapeList.Add(shape);
        }
    }

    class ChangeRadius : Change
    {
        private int deltaR;
        public ChangeRadius(int deltaR)
        {
            this.deltaR = deltaR;
        }

        public override void Redo()
        {
            Shape.Radius += deltaR;
        }
        public override void Undo()
        {
            Shape.Radius -= deltaR;
        }
    }

    class ChangeColor : Change
    {
        private Color last;
        private Color next;
        public ChangeColor(Color last, Color next)
        {
            this.last = last;
            this.next = next;
        }
        public override void Redo()
        {
            Shape.Color = next;
        }
        public override void Undo()
        {
            Shape.Color = last;
        }
    }

    class ChangeVertextype : Change
    {
        private ShapeType last;
        private ShapeType next;

        public ChangeVertextype(ShapeType last, ShapeType next)
        {
            this.last = last;
            this.next = next;
        }
        public override void Redo()
        {
            Form1.ShType = next;
        }
        public override void Undo()
        {
            Form1.ShType = last;
        }
    }

    class ChangeDynamic : Change
    {
        List<Shape> shapes;
        public ChangeDynamic(List<Shape> shapes)
        {
            this.shapes = new List<Shape>(shapes);
        }
        public override void Redo()
        {
            for (int i = 0; i < Form1.ShapeList.Count; i++)
            {
                Form1.ShapeList[i].X += Form1.ShapeList[i].X - shapes[i].X;
                Form1.ShapeList[i].Y += Form1.ShapeList[i].Y - shapes[i].Y;
            }
        }
        public override void Undo()
        {
            Form1.ShapeList = new List<Shape>(shapes);
        }
    }
}
