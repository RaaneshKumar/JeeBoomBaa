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
using Microsoft.Win32;
using System.IO;

namespace JeeBoomBaa;
public partial class CustomCanvas : Canvas {
   MyScribble mScribble = new ();
   MyRect mRect = new ();
   MyLine mLine = new ();
   List<MyDrawing> mDrawings = new ();
   Stack<MyDrawing> mRedoItems = new ();
   Queue<MyDrawing> mClearUndo = new ();

   public void ScribbleOn () { isScribbleOn = true; isRectOn = isLineOn = false; }
   bool isScribbleOn = true;

   public void RectOn () { isRectOn = true; isScribbleOn = isLineOn = false; }
   bool isRectOn = false;

   public void LineOn () { isLineOn = true; isRectOn = isScribbleOn = false; }
   bool isLineOn = false;

   public void EnableEraser () => isEraserEnabled = true;
   bool isEraserEnabled = false;

   public void EnableColor (EColor color) {
      isEraserEnabled = false;
      switch (color) {
         case Red: isRed = true; isGreen = isYellow = false; break;
         case Green: isGreen = true; isRed = isYellow = false; break;
         case Yellow: isYellow = true; isRed = isGreen = false; break;
         default: isRed = isGreen = isYellow = false; break;
      }
   }
   bool isRed = false, isGreen = false, isYellow = false;

   void SetPenColor (MyDrawing myDrawing) =>
      myDrawing.Color = isEraserEnabled ? Background : isRed ? Brushes.Red
                  : isGreen ? Brushes.Green : isYellow ? Brushes.Yellow : Brushes.White;

   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);

      for (int i = 0; i < mDrawings.Count; i++) {
         Pen mPen = new (mDrawings[i].Color, 10);
         switch (mDrawings[i]) {
            case MyScribble scribble:
               for (int j = 0; j < scribble.Points.Count - 1; j++) {
                  Point sStart = new (scribble.Points[j].X, scribble.Points[j].Y),
                        sEnd = new (scribble.Points[j + 1].X, scribble.Points[j + 1].Y);
                  dc.DrawLine (mPen, sStart, sEnd);
               }
               break;
            case MyLine line:
               MyPoint lStart = line.Points.start, lEnd = line.Points.end;
               dc.DrawLine (mPen, new(lStart.X, lStart.Y), new(lEnd.X, lEnd.Y));
               break;
            case MyRect myRect:
               MyPoint rStart = myRect.Points.start, rEnd = myRect.Points.end;
               Rect rectangle = new (new Point (rStart.X, rStart.Y), new Point (rEnd.X, rEnd.Y));
               dc.DrawRectangle (Brushes.Transparent, mPen, rectangle);
               break;
         }
      }
   }

   protected override void OnMouseLeftButtonDown (MouseButtonEventArgs e) {
      Point point = e.GetPosition (this);
      if (isScribbleOn) { SetPenColor (mScribble); mScribble.Points.Add (new (point.X, point.Y)); return; }
      if (isRectOn) { SetPenColor (mRect); mRect.Points.start = new (point.X, point.Y); }
      if (isLineOn) { SetPenColor (mLine); mLine.Points.start = new (point.X, point.Y); }
   }

   protected override void OnMouseMove (MouseEventArgs e) {
      if (Mouse.LeftButton != MouseButtonState.Pressed) return;
      Point point = e.GetPosition (this);
      if (isScribbleOn) mScribble.Points.Add (new (point.X, point.Y));
   }

   protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
      if (isScribbleOn) {
         mDrawings.Add (mScribble);
         mScribble = new ();
      }
      Point point = e.GetPosition (this);
      if (isRectOn) { mRect.Points.end = new (point.X, point.Y); mDrawings.Add (mRect); mRect = new (); }
      if (isLineOn) { mLine.Points.end = new (point.X, point.Y); mDrawings.Add (mLine); mLine = new (); }
      InvalidateVisual ();
   }

   public void ClearPoints () {
      foreach (var drawing in mDrawings)
         mClearUndo.Enqueue (drawing);
      mDrawings.Clear ();
      InvalidateVisual ();
   }

   public void Undo () {
      if (mClearUndo.Count != 0) {
         foreach (var drawing in mClearUndo)
            mDrawings.Add (drawing);
         InvalidateVisual ();
         return;
      }
      int count = mDrawings.Count;
      if (count == 0) return;
      mRedoItems.Push (mDrawings[^1]);
      mDrawings.RemoveAt (count - 1);
      InvalidateVisual ();
   }

   public void Redo () {
      if (mRedoItems.Count == 0) return;
      mDrawings.Add (mRedoItems.Pop ());
      InvalidateVisual ();
   }

   public void SaveAsText () {
      SaveFileDialog saveFile = new () {
         DefaultExt = "*.txt",
         Filter = "Text Document (*.txt)|*.txt|All (*.*)|*"
      };
      if (saveFile.ShowDialog () == true) {
         StreamWriter sw = new (saveFile.FileName, true);
         foreach (var drawing in mDrawings) {
            switch (drawing) {
               case MyScribble scribble:
                  sw.WriteLine ($"Scribble\n{scribble.Color}");
                  foreach (var point in scribble.Points) sw.WriteLine ($"{point.X},{point.Y}");
                  sw.WriteLine ();
                  break;
               case MyLine line:
                  sw.WriteLine ($"Line\n{line.Color}");
                  var lStart = line.Points.start;
                  var lEnd = line.Points.end;
                  sw.WriteLine ($"{lStart.X},{lStart.Y}\n{lEnd.X},{lEnd.Y}");
                  sw.WriteLine ();
                  break;
               case MyRect rect:
                  sw.WriteLine ($"Rect\n{rect.Color}");
                  var rStart = rect.Points.start;
                  var rEnd = rect.Points.end;
                  sw.WriteLine ($"{rStart.X},{rStart.Y}\n{rEnd.X},{rEnd.Y}");
                  sw.WriteLine ();
                  break;
            }
         }
         sw.Close ();
      }
   }

   public void LoadAsText () {
      mDrawings.Clear (); mRedoItems.Clear (); mClearUndo.Clear ();
      OpenFileDialog loadFile = new () {
         Filter = "Text Document (*.txt)|*.txt|All (*.*)|*"
      };
      if (loadFile.ShowDialog () is true) {
         MyScribble scribble = new (); MyRect rect = new (); MyLine myLine = new ();
         foreach (var line in File.ReadLines (loadFile.FileName)) {
            if (line == "Scribble") { isScribble = true; continue; }
            if (line == "Line") { isLine = true; continue; }
            if (line == "Rect") { isRect = true; continue; }
            if (isScribble) {
               if (line == "") { isScribble = false; mDrawings.Add (scribble); scribble = new (); continue; }
               if (line[0] == '#') {
                  scribble.Color = (Brush)new BrushConverter ().ConvertFrom (line); scribble.Thickness = 10;
                  continue;
               }
               var points = line.Split (',');
               scribble.Points.Add (new (double.Parse (points[0]), double.Parse (points[1])));
            } else if (isRect) {
               if (line == "") { isRect = false; mDrawings.Add (rect); rect = new (); continue; }
               if (line[0] == '#') {
                  rect.Color = (Brush)new BrushConverter ().ConvertFrom (line); rect.Thickness = 10;
                  continue;
               }
               var points = line.Split (',');
               if (rect.Points.start == null) rect.Points.start = new (double.Parse (points[0]), double.Parse (points[1]));
               else rect.Points.end = new (double.Parse (points[0]), double.Parse (points[1]));
            } else if (isLine) {
               if (line == "") { isLine = false; mDrawings.Add (myLine); myLine = new (); continue; }
               if (line[0] == '#') {
                  myLine.Color = (Brush)new BrushConverter ().ConvertFrom (line); myLine.Thickness = 10;
                  continue;
               }
               var points = line.Split (',');
               if (myLine.Points.start == null) myLine.Points.start = new (double.Parse (points[0]), double.Parse (points[1]));
               else myLine.Points.end = new (double.Parse (points[0]), double.Parse (points[1]));
            }
         }
      }
      InvalidateVisual ();
   }
   bool isScribble = false, isRect = false, isLine = false;

   public void SaveAsBin () {
      SaveFileDialog saveFile = new () {
         DefaultExt = "*.bin",
         Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
      };
      if (saveFile.ShowDialog () == true) {
         using (BinaryWriter bw = new (File.Create (saveFile.FileName))) {
            bw.Write (mDrawings.Count);
            foreach (var drawing in mDrawings) {
               SolidColorBrush sb = (SolidColorBrush)drawing.Color;
               bw.Write (sb.Color.A); bw.Write (sb.Color.R);
               bw.Write (sb.Color.G); bw.Write (sb.Color.B);
               switch (drawing) {
                  case MyScribble scribble:
                     bw.Write (scribble.Rank);
                     bw.Write (scribble.Points.Count);
                     foreach (var point in scribble.Points) {
                        bw.Write (point.X); bw.Write (point.Y);
                     }
                     break;
                  case MyLine line:
                     bw.Write (line.Rank);
                     MyPoint lStart = line.Points.start,
                             lEnd = line.Points.end;
                     bw.Write (lStart.X); bw.Write (lStart.Y);
                     bw.Write (lEnd.X); bw.Write (lEnd.Y);
                     break;
                  case MyRect rect:
                     bw.Write (rect.Rank);
                     MyPoint rStart = rect.Points.start,
                             rEnd = rect.Points.end;
                     bw.Write (rStart.X); bw.Write (rStart.Y);
                     bw.Write (rEnd.X); bw.Write (rEnd.Y);
                     break;
               }
            }
         }
      }
   }

   public void LoadAsBin () {
      mDrawings.Clear (); mRedoItems.Clear (); mClearUndo.Clear ();
      OpenFileDialog loadFile = new () {
         Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
      };
      if (loadFile.ShowDialog () == true) {
         using (BinaryReader br = new (File.Open (loadFile.FileName, FileMode.Open))) {
            var drawingCount = br.ReadInt32 ();
            for (int i = 0; i < drawingCount; i++) {
               byte a = br.ReadByte (), r = br.ReadByte (),
                      g = br.ReadByte (), b = br.ReadByte ();
               var rank = br.ReadInt32 ();
               switch (rank) {
                  case 0:
                     MyScribble scribble = new () {
                        Color = new SolidColorBrush (Color.FromArgb (a, r, g, b))
                     };
                     var pointCount = br.ReadInt32 ();
                     for (int j = 0; j < pointCount; j++)
                        scribble.Points.Add (new (br.ReadDouble (), br.ReadDouble ()));
                     mDrawings.Add (scribble);
                     break;
                  case 1:
                     MyLine line = new () {
                        Color = new SolidColorBrush (Color.FromArgb (a, r, g, b)),
                        Points = (new (br.ReadDouble (), br.ReadDouble ()), new (br.ReadDouble (), br.ReadDouble ()))
                     };
                     mDrawings.Add (line);
                     break;
                  case 2:
                     MyRect rect = new () {
                        Color = new SolidColorBrush (Color.FromArgb (a, r, g, b)),
                        Points = (new (br.ReadDouble (), br.ReadDouble ()), new (br.ReadDouble (), br.ReadDouble ()))
                     };
                     mDrawings.Add (rect);
                     break;
               }
            }
         }
      }
      InvalidateVisual ();
   }
}

public enum EColor { White, Red, Green, Yellow }