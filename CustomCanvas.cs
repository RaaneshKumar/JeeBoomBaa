using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using static JeeBoomBaa.EColor;

namespace JeeBoomBaa;
public partial class CustomCanvas : Canvas {
   MyLine mLine = new ();
   List<MyDrawing> mDrawings = new ();
   Stack<MyDrawing> mRedoItems = new ();
   Pen mPen;

   public void EnableEraser () { mLine.Color = Background; }
   public void EnableColor (EColor color) { 
      switch (color) {
         case Red: mLine.Color = Brushes.Red; break;
         case Green: mLine.Color = Brushes.Green; break;
         case Yellow: mLine.Color = Brushes.Yellow; break;
      }
   }

   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);

      for (int i = 0; i < mDrawings.Count; i++) {
         if (mDrawings[i] is MyLine line) {
            for (int j = 0; j < line.Points.Count - 1; j++) {
               mPen = new (line.Color, 10);
               Point start = new (line.Points[j].X, line.Points[j].Y);
               Point end = new (line.Points[j+1].X, line.Points[j+1].Y);
               dc.DrawLine (mPen, start, end);
            }
         }
      }
   }

   protected override void OnMouseLeftButtonDown (MouseButtonEventArgs e) {
      Point point = e.GetPosition (this);
      mLine.Points.Add (new (point.X, point.Y));
   }

   protected override void OnMouseMove (MouseEventArgs e) {
      if (Mouse.LeftButton != MouseButtonState.Pressed) return;
      Point point = e.GetPosition (this);
      mLine.Points.Add (new (point.X, point.Y));
   }

   protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
      mDrawings.Add (mLine);
      mLine = new ();
      InvalidateVisual ();
   }

   public void ClearPoints () { mDrawings.Clear (); InvalidateVisual (); }

   public void Undo () {
      mRedoItems.Push (mDrawings[^1]);
      mDrawings.RemoveAt (mDrawings.Count - 1);
      InvalidateVisual ();
   }

   public void Redo () {
      mDrawings.Add (mRedoItems.Pop ());
      InvalidateVisual ();
   }
}
public enum EColor { White, Red, Green, Yellow }