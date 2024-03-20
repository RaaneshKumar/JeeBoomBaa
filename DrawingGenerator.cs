using System.Windows;
using System.Windows.Media;
using static JeeBoomBaa.EColor;

namespace JeeBoomBaa {
   #region Drawing Generator ----------------------------------------------------------------------
   internal class DrawingGenerator {
      #region Constructors ------------------------------------------
      public DrawingGenerator (DrawingContext dc) {
         mDC = dc;
      }
      DrawingContext mDC;
      #endregion

      #region Methods -----------------------------------------------
      public void Draw (MyDrawing dwg) => Draw (dwg as dynamic, SetBrushColor (dwg));

      public void Draw (MyScribble scr, Brush brush) {
         for (int j = 0; j < scr.PointList.Count - 1; j++) {
            Point sStart = new (scr.PointList[j].X, scr.PointList[j].Y),
                  sEnd = new (scr.PointList[j + 1].X, scr.PointList[j + 1].Y);
            mDC.DrawLine (new Pen (brush, 2), sStart, sEnd);
         }
      }

      public void Draw (MyLine line, Brush brush) {
         MyPoint lStart = line.PointList[0], lEnd = line.PointList[^1];
         mDC.DrawLine (new Pen (brush, 2), new Point (lStart.X, lStart.Y), new Point (lEnd.X, lEnd.Y));
      }

      public void Draw (MyRect rect, Brush brush) {
         MyPoint rStart = rect.PointList[0], rEnd = rect.PointList[^1];
         Rect rectangle = new (new Point (rStart.X, rStart.Y), new Point (rEnd.X, rEnd.Y));
         mDC.DrawRectangle (Brushes.Transparent, new Pen (brush, 2), rectangle);
      }

      public void Draw (MyConnectedLine conLine, Brush brush) {
         for (int j = 0; j < conLine.PointList.Count - 1; j++) {
            Point sStart = new (conLine.PointList[j].X, conLine.PointList[j].Y),
                  sEnd = new (conLine.PointList[j + 1].X, conLine.PointList[j + 1].Y);
            mDC.DrawLine (new Pen (brush, 2), sStart, sEnd);
         }
      }

      public Brush SetBrushColor (MyDrawing dwg) {
         Brush brush = dwg.MyBrushColor switch {
            Red => Brushes.Red,
            Green => Brushes.Green,
            Yellow => Brushes.Yellow,
            _ => Brushes.White,
         };
         return brush;
      }
      #endregion
   }
   #endregion
}