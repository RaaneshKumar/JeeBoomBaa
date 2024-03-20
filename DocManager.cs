using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeeBoomBaa {
   public class DocManager {
      #region Constructors ------------------------------------------
      public DocManager (CustomCanvas canvas) {
         mDrawings = canvas.Drawings;
         mCanvas = canvas;
      }
      List<MyDrawing> mDrawings;
      CustomCanvas mCanvas;
      #endregion

      #region Methods -----------------------------------------------
      public void SaveFile () {
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

      public void OpenFile () {
         mCanvas.ClearData ();
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
         mCanvas.InvalidateVisual ();
         isLoaded = true;
         mLoadCount = mDrawings.Count;
      }

      public bool IsLoadedFile (out int count) {
         count = mLoadCount;
         return isLoaded;
      }
      #endregion

      #region Private -----------------------------------------------
      bool isLoaded = false; // To keep track of new file or loaded file
      int mLoadCount; // To keep track of drawings count in the loaded file
      #endregion
   }
}