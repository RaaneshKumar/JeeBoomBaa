using System.Windows.Media;
using System.Windows;

namespace JeeBoomBaa {
   public class MyScribble : MyDrawing {
      public override int Rank => 0;
      public double Thickness { get; set; }

      public override void Draw (DrawingContext dc) {
         for (int j = 0; j < PointList.Count - 1; j++) {
            Point sStart = new (PointList[j].X, PointList[j].Y),
                  sEnd = new (PointList[j + 1].X, PointList[j + 1].Y);
            dc.DrawLine (new Pen (MyBrush, 2), sStart, sEnd);
         }
      }
   }

   public class MyRect : MyDrawing {
      public override int Rank => 2;

      public double Thickness { get; set; }

      public override void Draw (DrawingContext dc) {
         MyPoint rStart = PointList[0], rEnd = PointList[^1];
         Rect rectangle = new (new Point (rStart.X, rStart.Y), new Point (rEnd.X, rEnd.Y));
         dc.DrawRectangle (Brushes.Transparent, new Pen (MyBrush, 2), rectangle);
      }
   }

   public class MyLine : MyDrawing {
      public override int Rank => 1;

      public double Thickness { get; set; }

      public override void Draw (DrawingContext dc) {
         MyPoint lStart = PointList[0], lEnd = PointList[^1];
         dc.DrawLine (new Pen (MyBrush, 2), new Point (lStart.X, lStart.Y), new Point (lEnd.X, lEnd.Y));
      }
   }

   public class MyConnectedLine : MyDrawing {
      public override int Rank => 3;
      public double Thickness { get; set; }

      public override void Draw (DrawingContext dc) {
         for (int j = 0; j < PointList.Count - 1; j++) {
            Point sStart = new (PointList[j].X, PointList[j].Y),
                  sEnd = new (PointList[j + 1].X, PointList[j + 1].Y);
            dc.DrawLine (new Pen (MyBrush, 2), sStart, sEnd);
         }
      }
   }
}