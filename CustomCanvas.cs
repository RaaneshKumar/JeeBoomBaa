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
   MyLine mLine = new ();
   List<MyDrawing> mDrawings = new ();
   Stack<MyDrawing> mRedoItems = new ();
   Queue<MyDrawing> mClearUndo = new ();

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

   void SetPenColor () =>
      mLine.Color = isEraserEnabled ? Background : isRed ? Brushes.Red
                  : isGreen ? Brushes.Green : isYellow ? Brushes.Yellow : Brushes.White;

   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);

      for (int i = 0; i < mDrawings.Count; i++) {
         if (mDrawings[i] is MyLine line) {
            for (int j = 0; j < line.Points.Count - 1; j++) {
               Pen mPen = new (line.Color, 10);
               Point start = new (line.Points[j].X, line.Points[j].Y);
               Point end = new (line.Points[j + 1].X, line.Points[j + 1].Y);
               dc.DrawLine (mPen, start, end);
            }
         }
      }
   }

   protected override void OnMouseLeftButtonDown (MouseButtonEventArgs e) {
      SetPenColor ();
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
            if (drawing is MyLine line) {
               sw.WriteLine ($"Scribble\n{line.Color}");
               foreach (var point in line.Points) sw.WriteLine ($"{point.X},{point.Y}");
               sw.WriteLine ();
            }
         }
         sw.Close ();
      }
   }

   public void LoadAsText () {
      OpenFileDialog loadFile = new () {
         Filter = "Text Document (*.txt)|*.txt|All (*.*)|*"
      };
      if (loadFile.ShowDialog () is true) {
         var fileName = loadFile.FileName;
         MyLine scribble = new ();
         foreach (var line in File.ReadLines (fileName)) {
            if (line == "Scribble") { isScribble = true; continue; }
            if (isScribble) {
               if (line == "") { isScribble = false; mDrawings.Add (scribble); scribble = new (); continue; }
               if (line[0] == '#') { scribble.Color = (Brush)new BrushConverter ().ConvertFrom (line); scribble.Thickness = 10; continue; }
               var points = line.Split (',');
               scribble.Points.Add (new (double.Parse (points[0]), double.Parse (points[1])));
            }
         }
      }
      InvalidateVisual ();
   }
   bool isScribble = false;

   public void SaveAsBin () {
      SaveFileDialog saveFile = new () {
         DefaultExt = "*.bin",
         Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
      };
      if (saveFile.ShowDialog () == true) {
         using (BinaryWriter bw = new (File.Create (saveFile.FileName))) {
            bw.Write (mDrawings.Count);
            foreach (var drawing in mDrawings)
               if (drawing is MyLine line) {
                  SolidColorBrush sb = (SolidColorBrush)line.Color;
                  bw.Write (sb.Color.A);
                  bw.Write (sb.Color.R);
                  bw.Write (sb.Color.G);
                  bw.Write (sb.Color.B);
                  bw.Write (line.Points.Count);
                  foreach (var point in line.Points) {
                     bw.Write (point.X);
                     bw.Write (point.Y);
                  }
               }
         }
      }
   }

   public void LoadAsBin () {
      OpenFileDialog loadFile = new () {
         Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
      };
      if (loadFile.ShowDialog () == true) {
         using (BinaryReader br = new (File.Open(loadFile.FileName, FileMode.Open))) {
            var drawingCount = br.ReadInt32 ();
            for (int i = 0; i < drawingCount; i++) {
               MyLine scribble = new ();
               var a = br.ReadByte ();
               var r = br.ReadByte ();
               var g = br.ReadByte ();
               var b = br.ReadByte ();
               scribble.Color = new SolidColorBrush (Color.FromArgb (a, r, g, b));
               var pointCount = br.ReadInt32 ();
               for (int j = 0; j < pointCount; j++) 
                  scribble.Points.Add (new (br.ReadDouble (), br.ReadDouble ()));
               mDrawings.Add (scribble);
            }
         }
      }
      InvalidateVisual ();
   }
}
public enum EColor { White, Red, Green, Yellow }