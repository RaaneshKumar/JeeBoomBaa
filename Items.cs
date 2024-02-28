using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using static JeeBoomBaa.EColor;

namespace JeeBoomBaa {
   public class MyPoint {
      public MyPoint (double pX, double pY) {
         X = pX; Y = pY;
      }

      public double X {
         get => mX;
         set => mX = value;
      }
      double mX = double.NaN;

      public double Y {
         get => mY;
         set => mY = value;
      }
      double mY = double.NaN;
   }

   public class MyLine : MyDrawing {
      public int Rank => 1;

      public double Thickness { get; set; }

      public override void Draw (DrawingContext dc) {
         MyPoint lStart = PointList[0], lEnd = PointList[^1];
         dc.DrawLine (new Pen (MyBrush, 2), new Point (lStart.X, lStart.Y), new Point (lEnd.X, lEnd.Y));
      }
   }

   public class MyConnectedLine : MyDrawing {
      public int Rank => 3;
      public double Thickness { get; set; }

      public override void Draw (DrawingContext dc) {
         for (int j = 0; j < PointList.Count - 1; j++) {
            Point sStart = new (PointList[j].X, PointList[j].Y),
                  sEnd = new (PointList[j + 1].X, PointList[j + 1].Y);
            dc.DrawLine (new Pen (MyBrush, 2), sStart, sEnd);
         }
      }
   }

   public class MyScribble : MyDrawing {
      public int Rank => 0;
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
      public int Rank => 2;

      public double Thickness { get; set; }

      public override void Draw (DrawingContext dc) {
         MyPoint rStart = PointList[0], rEnd = PointList[^1];
         Rect rectangle = new (new Point (rStart.X, rStart.Y), new Point (rEnd.X, rEnd.Y));
         dc.DrawRectangle (Brushes.Transparent, new Pen (MyBrush, 2), rectangle);
      }
   }

   public class MyDrawing {
      public Brush MyBrush {
         get {
            return Color switch {
               Red => Brushes.Red,
               Green => Brushes.Green,
               Yellow => Brushes.Yellow,
               _ => Brushes.White,
            };
            //return mBrush;
         }
         set => mBrush = value;
      }
      Brush mBrush;

      public EColor Color { get; set; }

      public virtual void Draw (DrawingContext dc) { }

      public EShape Shape { get; set; }

      public List<MyPoint> PointList = new ();

      public (MyPoint start, MyPoint end) Points = new ();
   }

   public enum EShape { SCRIBBLE, LINE, RECTANGLE, CONNECTEDLINE, }

   public enum EColor { White, Red, Green, Yellow }
}