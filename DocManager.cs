using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JeeBoomBaa {
   public class DocManager {
      #region Methods -----------------------------------------------
      public void SaveFile (List<MyDrawing> drawings) {
         SaveFileDialog saveFile = new () {
            DefaultExt = "*.bin",
            Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
         };
         if (saveFile.ShowDialog () == true) {
            using (BinaryWriter bw = new (File.Create (saveFile.FileName))) {
               bw.Write (drawings.Count);
               foreach (var drawing in drawings) drawing.SaveAsBin (bw);
            }
         }
      }

      public void OpenFile (out List<MyDrawing> drawings) {
         mDrawings.Clear ();
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
         isLoaded = true;
         mLoadCount = mDrawings.Count;
         drawings = new ();
         for (int i = 0; i < mDrawings.Count; i++) drawings.Add (mDrawings[i]);
      }

      public bool IsLoadedFile (out int count) {
         count = mLoadCount;
         return isLoaded;
      }
      #endregion

      #region Private -----------------------------------------------
      List<MyDrawing> mDrawings = new ();
      bool isLoaded = false; // To keep track of new file or loaded file
      int mLoadCount; // To keep track of drawings count in the loaded file
      #endregion
   }
}