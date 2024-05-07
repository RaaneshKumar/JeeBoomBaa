namespace JeeBoomBaa {
   #region Shapes ---------------------------------------------------------------------------------
   public class Scribble : Shape {
      public override void Draw (IDrawable shape) {
         throw new NotImplementedException ();
      }
   }

   public class Rectangle : Shape {
      public override void Draw (IDrawable shape) => shape.DrawRect (this);
   }

   public class Line : Shape {
      public override void Draw (IDrawable shape) => shape.DrawLine (this);
   }

   public class ConnectedLine : Shape {
      public override void Draw (IDrawable shape) => shape.DrawConLine (this);
   }
   #endregion
}