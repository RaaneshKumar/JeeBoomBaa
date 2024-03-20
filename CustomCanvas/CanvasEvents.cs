using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using static JeeBoomBaa.EShape;

namespace JeeBoomBaa {
   #region Custom Canvas --------------------------------------------------------------------------
   public partial class CustomCanvas : Canvas {
      #region Implementation ----------------------------------------
      protected override void OnRender (DrawingContext dc) {
         base.OnRender (dc);
         DrawingGenerator dg = new (dc);
         for (int i = 0; i < mDrawings.Count; i++) dg.Draw (mDrawings[i]);
         if (mDrawing.PointList.Count != 0) dg.Draw (mDrawing);
      }
      #endregion

      #region Mouse Events ------------------------------------------
      protected override void OnMouseLeftButtonDown (MouseButtonEventArgs e) {
         mRedoItems.Clear ();
         Point point = e.GetPosition (this);
         switch (mDrawing.Shape) {
            case SCRIBBLE:
               mDrawing.PointList.Add (new (point.X, point.Y));
               break;
            case RECTANGLE or LINE:
               for (int i = 0; i < 2; i++)
                  mDrawing.PointList.Add (new (point.X, point.Y));
               break;
            case CONNECTEDLINE:
               for (int i = 0; i < 2; i++)
                  mDrawing.PointList.Add (new (point.X, point.Y));
               break;
         }
      }

      protected override void OnMouseMove (MouseEventArgs e) {
         if (mDrawing.Shape is not CONNECTEDLINE && Mouse.LeftButton != MouseButtonState.Pressed) return;
         Point point = e.GetPosition (this);
         switch (mDrawing.Shape) {
            case SCRIBBLE:
               if (mDrawing.PointList.Count > 0) mDrawing.PointList.Add (new (point.X, point.Y));
               break;
            case RECTANGLE or LINE or CONNECTEDLINE:
               if (mDrawing.PointList.Count > 0) mDrawing.PointList[^1] = new (point.X, point.Y);
               break;
         }
         InvalidateVisual ();
      }

      protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
         Point point = e.GetPosition (this);
         switch (mDrawing.Shape) {
            case SCRIBBLE:
               mDrawings.Add (mDrawing);
               mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrushColor = mBrush };
               break;
            case RECTANGLE:
               mDrawing.PointList[^1] = new (point.X, point.Y);
               mDrawings.Add (mDrawing);
               mDrawing = new MyRect () { Shape = RECTANGLE, MyBrushColor = mBrush };
               break;
            case LINE:
               mDrawing.PointList[^1] = new (point.X, point.Y);
               mDrawings.Add (mDrawing);
               mDrawing = new MyLine () { Shape = LINE, MyBrushColor = mBrush };
               break;
         }
         InvalidateVisual ();
      }
      #endregion

      #region Keyboard Events ---------------------------------------
      public void KeyPressed (Key k) {
         if (mDrawing.Shape is CONNECTEDLINE && mDrawing.PointList.Count > 0 && k == Key.Escape) {
            mDrawing.PointList[^1] = mDrawing.PointList[^2];
            mDrawings.Add (mDrawing);
            mDrawing = new MyConnectedLine { Shape = CONNECTEDLINE, MyBrushColor = mBrush };
         }
         InvalidateVisual ();
      }
      #endregion
   }
   #endregion
}