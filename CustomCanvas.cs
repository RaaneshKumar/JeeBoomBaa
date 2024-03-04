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
using static JeeBoomBaa.EShape;
using Microsoft.Win32;
using System.IO;

namespace JeeBoomBaa {
   #region Classes --------------------------------------------------------------------------------
   public partial class CustomCanvas : Canvas {
      Brush mBrush = Brushes.White;
      MyDrawing mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrush = Brushes.White };
      List<MyDrawing> mDrawings = new ();
      Stack<MyDrawing> mRedoItems = new ();
      Queue<MyDrawing> mClearUndo = new ();

      #region Functions ---------------------------------------------
      public void ScribbleOn () { mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrush = mBrush }; }

      public void RectOn () { mDrawing = new MyRect { Shape = RECTANGLE, MyBrush = mBrush }; }

      public void LineOn () { mDrawing = new MyLine { Shape = LINE, MyBrush = mBrush }; }

      public void ConnectedLineOn () { mDrawing = new MyConnectedLine { Shape = CONNECTEDLINE, MyBrush = mBrush }; }

      public void ChangeColor (EColor color) {
         mBrush = color switch {
            Red => Brushes.Red,
            Green => Brushes.Green,
            Yellow => Brushes.Yellow,
            _ => Brushes.White,
         };
         mDrawing.MyBrush = mBrush;
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
      #endregion

      #region Implementation ----------------------------------------
      protected override void OnRender (DrawingContext dc) {
         base.OnRender (dc);
         for (int i = 0; i < mDrawings.Count; i++) mDrawings[i].Draw (dc);
      }
      #endregion

      #region Event handlers ----------------------------------------
      protected override void OnMouseLeftButtonDown (MouseButtonEventArgs e) {
         Point point = e.GetPosition (this);
         switch (mDrawing.Shape) {
            case SCRIBBLE:
               mDrawing.PointList.Add (new (point.X, point.Y));
               mDrawings.Add (mDrawing);
               break;
            case RECTANGLE:
               //mDrawing.Points.start = new (point.X, point.Y);
               mDrawing.PointList.Add (new (point.X, point.Y));
               mDrawing.PointList.Add (null);
               mDrawings.Add (mDrawing);
               break;
            case LINE:
               //mDrawing.Points.start = new (point.X, point.Y);
               mDrawing.PointList.Add (new (point.X, point.Y));
               mDrawing.PointList.Add (null);
               mDrawings.Add (mDrawing);
               break;
            case CONNECTEDLINE:
               MyPoint pt = new (point.X, point.Y);
               for (int i = 0; i < 2; i++) mDrawing.PointList.Add (pt);
               mDrawings.Add (mDrawing);
               break;
         }
      }

      protected override void OnMouseMove (MouseEventArgs e) {
         if (mDrawing.Shape is not CONNECTEDLINE && Mouse.LeftButton != MouseButtonState.Pressed) return;
         Point point = e.GetPosition (this);
         switch (mDrawing.Shape) {
            case SCRIBBLE:
               if (mDrawing.PointList.Count > 0) {
                  mDrawing.PointList.Add (new (point.X, point.Y));
                  mDrawings[^1] = mDrawing;
               }
               break;
            case RECTANGLE or LINE or CONNECTEDLINE:
               if (mDrawing.PointList.Count > 0) {
                  mDrawing.PointList[^1] = new (point.X, point.Y);
                  mDrawings[^1] = mDrawing;
               }
               break;
         }
         InvalidateVisual ();
      }

      protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
         Point point = e.GetPosition (this);
         switch (mDrawing.Shape) {
            case SCRIBBLE:
               mDrawings[^1] = mDrawing;
               mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrush = mBrush };
               break;
            case RECTANGLE:
               mDrawing.PointList[^1] = new (point.X, point.Y);
               mDrawings[^1] = mDrawing;
               mDrawing = new MyRect () { Shape = RECTANGLE, MyBrush = mBrush };
               break;
            case LINE:
               mDrawing.PointList[^1] = new (point.X, point.Y);
               mDrawings[^1] = mDrawing;
               mDrawing = new MyLine () { Shape = LINE, MyBrush = mBrush };
               break;
         }
         InvalidateVisual ();
      }

      public void KeyPressed (Key k) {
         if (mDrawing.Shape is CONNECTEDLINE && mDrawing.PointList.Count > 0 && k == Key.Escape) {
            mDrawing.PointList[^1] = mDrawing.PointList[^2];
            mDrawings[^1] = mDrawing;
            mDrawing = new MyConnectedLine { Shape = CONNECTEDLINE, MyBrush = mBrush };
         }
         InvalidateVisual ();
      }
      #endregion

      #region Save and Load -----------------------------------------
      public void SaveAsText () {
         SaveFileDialog saveFile = new () {
            DefaultExt = "*.txt",
            Filter = "Text Document (*.txt)|*.txt|All (*.*)|*"
         };
         if (saveFile.ShowDialog () == true) {
            StreamWriter sw = new (saveFile.FileName, true);
            foreach (var drawing in mDrawings) {
               sw.WriteLine ($"{drawing.Shape}\n{drawing.MyBrush}");
               foreach (var point in drawing.PointList) sw.WriteLine ($"{point.X},{point.Y}");
               sw.WriteLine ();
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
            MyDrawing drawing = new ();
            foreach (var line in File.ReadLines (loadFile.FileName)) {
               if (line == "SCRIBBLE") { drawing = new MyScribble (); continue; }
               if (line == "LINE") { drawing = new MyLine (); continue; }
               if (line == "RECTANGLE") { drawing = new MyRect (); continue; }
               if (line == "CONNECTEDLINE") { drawing = new MyConnectedLine (); continue; }
               if (line == "") { mDrawings.Add (drawing); continue; }
               if (line[0] == '#') { drawing.MyBrush = (Brush)new BrushConverter ().ConvertFrom (line); continue; }
               var points = line.Split (',');
               drawing.PointList.Add (new (double.Parse (points[0]), double.Parse (points[1])));
            }
         }
         InvalidateVisual ();
      }

      public void SaveAsBin () {
         SaveFileDialog saveFile = new () {
            DefaultExt = "*.bin",
            Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
         };
         if (saveFile.ShowDialog () == true) {
            using (BinaryWriter bw = new (File.Create (saveFile.FileName))) {
               bw.Write (mDrawings.Count);
               foreach (var drawing in mDrawings) {
                  SolidColorBrush sb = (SolidColorBrush)drawing.MyBrush;
                  bw.Write (sb.Color.A); bw.Write (sb.Color.R);
                  bw.Write (sb.Color.G); bw.Write (sb.Color.B);
                  switch (drawing) {
                     case MyScribble scribble:
                        bw.Write (scribble.Rank);
                        bw.Write (scribble.PointList.Count);
                        foreach (var point in scribble.PointList) {
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
                           MyBrush = new SolidColorBrush (Color.FromArgb (a, r, g, b))
                        };
                        var pointCount = br.ReadInt32 ();
                        for (int j = 0; j < pointCount; j++)
                           scribble.PointList.Add (new (br.ReadDouble (), br.ReadDouble ()));
                        mDrawings.Add (scribble);
                        break;
                     case 1:
                        MyLine line = new () {
                           MyBrush = new SolidColorBrush (Color.FromArgb (a, r, g, b)),
                           Points = (new (br.ReadDouble (), br.ReadDouble ()), new (br.ReadDouble (), br.ReadDouble ()))
                        };
                        mDrawings.Add (line);
                        break;
                     case 2:
                        MyRect rect = new () {
                           MyBrush = new SolidColorBrush (Color.FromArgb (a, r, g, b)),
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
      #endregion
   }
   #endregion
}