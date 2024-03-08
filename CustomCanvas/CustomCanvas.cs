using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using static JeeBoomBaa.EColor;
using static JeeBoomBaa.EShape;
using Microsoft.Win32;
using System.IO;

namespace JeeBoomBaa {
   #region Classes --------------------------------------------------------------------------------
   public partial class CustomCanvas : Canvas {
      Brush mBrush = Brushes.White;
      protected MyDrawing mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrush = Brushes.White };
      List<MyDrawing> mDrawings = new ();
      protected Stack<MyDrawing> mRedoItems = new ();
      Queue<MyDrawing> mClearUndo = new ();

      #region Functions ---------------------------------------------
      public void ScribbleOn () => mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrush = mBrush };

      public void RectOn () => mDrawing = new MyRect { Shape = RECTANGLE, MyBrush = mBrush };

      public void LineOn () => mDrawing = new MyLine { Shape = LINE, MyBrush = mBrush };

      public void ConnectedLineOn () => mDrawing = new MyConnectedLine { Shape = CONNECTEDLINE, MyBrush = mBrush };

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
         // For performing undo over clear operation
         if (mClearUndo.Count != 0) {
            foreach (var drawing in mClearUndo) mDrawings.Add (drawing);
            InvalidateVisual ();
            return;
         }
         // Implementing undo for each drawing
         var count = mDrawings.Count;
         if ((isLoaded && count == loadCount) || count == 0) return;
         mRedoItems.Push (mDrawings[^1]);
         mDrawings.RemoveAt (mDrawings.Count - 1);
         InvalidateVisual ();
      }

      public void Redo () {
         if (mRedoItems.Count == 0) return;
         mDrawings.Add (mRedoItems.Pop ());
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
            foreach (var drawing in mDrawings) drawing.SaveAsText (sw);
            sw.Close ();
         }
      }

      /// <summary>Loads the saved text files and converts them into drawings</summary>
      public void LoadAsText () {
         mDrawings.Clear (); mRedoItems.Clear (); mClearUndo.Clear (); // Clearing the current data before loading
         OpenFileDialog loadFile = new () {
            Filter = "Text Document (*.txt)|*.txt|All (*.*)|*"
         };
         if (loadFile.ShowDialog () is true) {
            MyDrawing drawing = new ();
            foreach (var line in File.ReadLines (loadFile.FileName)) {
               switch (line) { // Reads the type of drawing and creates the corresponding object
                  case "SCRIBBLE": drawing = new MyScribble (); continue;
                  case "LINE": drawing = new MyLine (); continue;
                  case "RECTANGLE": drawing = new MyRect (); continue;
                  case "CONNECTEDLINE": drawing = new MyConnectedLine (); continue;
                  case "": mDrawings.Add (drawing); continue;
               } // Since this brush will never be null
               if (line[0] == '#') { drawing.MyBrush = (Brush)new BrushConverter ().ConvertFrom (line)!; continue; }
               var points = line.Split (',');
               drawing.PointList.Add (new (double.Parse (points[0]), double.Parse (points[1])));
            }
         }
         InvalidateVisual ();
         isLoaded = true;
         loadCount = mDrawings.Count;
      }

      public void SaveAsBin () {
         SaveFileDialog saveFile = new () {
            DefaultExt = "*.bin",
            Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
         };
         if (saveFile.ShowDialog () == true) {
            using (BinaryWriter bw = new (File.Create (saveFile.FileName))) {
               bw.Write (mDrawings.Count);
               foreach (var drawing in mDrawings) drawing.SaveAsBin (bw);
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
               MyDrawing drawing = new ();
               for (int i = 0; i < drawingCount; i++) mDrawings.Add (drawing.LoadBin (br));
            }
         }
         InvalidateVisual ();
         isLoaded = true;
         loadCount = mDrawings.Count;
      }
      bool isLoaded = false; // To keep track of new file or loaded file
      int loadCount; // To keep track of drawings count in the loaded file
      #endregion
   }
   #endregion
}