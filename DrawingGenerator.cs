using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using static JeeBoomBaa.EColor;

namespace JeeBoomBaa {
   #region Drawing Generator ----------------------------------------------------------------------
   public class DrawingGenerator : IDrawable {
      #region Constructors ------------------------------------------
      public DrawingGenerator (DrawingContext dc) {
         mDC = dc;
      }
      DrawingContext mDC;
      #endregion

      #region Methods -----------------------------------------------
      public void Draw (Drawing dwg) {
         foreach (var shape in dwg.ShapeList) {
            mBrush = SetBrushColor (shape);
            switch (shape) {
               case Line line: DrawLine (line); break;
               case Rectangle rect: DrawRect (rect); break;
               case ConnectedLine conLine: DrawConLine (conLine); break;
            }
         }
      }

      public void Draw (Shape shape) {
         mBrush = SetBrushColor (shape);
         switch (shape) {
            case Line line: DrawLine (line); break;
            case Rectangle rect: DrawRect (rect); break;
            case ConnectedLine conLine: DrawConLine (conLine); break;
         }
      }

      public Brush SetBrushColor (Shape s) {
         Brush brush = s.MyBrushColor switch {
            Red => Brushes.Red,
            Green => Brushes.Green,
            Yellow => Brushes.Yellow,
            _ => Brushes.Black,
         };
         return brush;
      }

      public void DrawLine (Line line) {
         Point lStart = line.PointList[0], lEnd = line.PointList[^1];
         mDC.DrawLine (new Pen (mBrush, 2), new System.Windows.Point (lStart.X, lStart.Y), new System.Windows.Point (lEnd.X, lEnd.Y));
      }

      public void DrawRect (Rectangle rect) {
         Point rStart = rect.PointList[0], rEnd = rect.PointList[^1];
         Rect rectangle = new (new System.Windows.Point (rStart.X, rStart.Y), new System.Windows.Point (rEnd.X, rEnd.Y));
         mDC.DrawRectangle (Brushes.Transparent, new Pen (mBrush, 2), rectangle);
      }

      public void DrawConLine (ConnectedLine conLine) {
         for (int j = 0; j < conLine.PointList.Count - 1; j++) {
            System.Windows.Point sStart = new (conLine.PointList[j].X, conLine.PointList[j].Y),
                  sEnd = new (conLine.PointList[j + 1].X, conLine.PointList[j + 1].Y);
            mDC.DrawLine (new Pen (mBrush, 2), sStart, sEnd);
         }
      }
      #region Private ---------
      Brush? mBrush;
      #endregion
      #endregion
   }
   #endregion
}