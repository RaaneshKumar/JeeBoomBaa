using System.Collections.Generic;
using System.Windows.Controls;
using static JeeBoomBaa.EColor;
using static JeeBoomBaa.EShape;
using Microsoft.Win32;
using System.IO;

namespace JeeBoomBaa {
   #region Custom Canvas --------------------------------------------------------------------------
   public partial class CustomCanvas : Canvas {
      #region Methods -----------------------------------------------
      public void ScribbleOn () => mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrushColor = mBrush };

      public void RectOn () => mDrawing = new MyRect { Shape = RECTANGLE, MyBrushColor = mBrush };

      public void LineOn () => mDrawing = new MyLine { Shape = LINE, MyBrushColor = mBrush };

      public void ConnectedLineOn () => mDrawing = new MyConnectedLine { Shape = CONNECTEDLINE, MyBrushColor = mBrush };

      public void ChangeColor (EColor color) {
         mBrush = color;
         mDrawing.MyBrushColor = mBrush;
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

      public void New () {
         mDrawings.Clear (); mRedoItems.Clear (); mClearUndo.Clear ();
         mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrushColor = White };
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

      #region Private -----------------------------------------------
      EColor mBrush = White;
      protected MyDrawing mDrawing = new MyScribble { Shape = SCRIBBLE, MyBrushColor = White };
      List<MyDrawing> mDrawings = new ();
      protected Stack<MyDrawing> mRedoItems = new ();
      Queue<MyDrawing> mClearUndo = new ();
      #endregion
   }
   #endregion
}