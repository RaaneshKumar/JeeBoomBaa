using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace JeeBoomBaa {
   public class DocManager {
      #region Methods -----------------------------------------------
      public void SaveAs (Drawing dwg) {
         SaveFileDialog saveFile = new () {
            DefaultExt = "*.bin",
            Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
         };
         if (saveFile.ShowDialog () == true) {
            using (BinaryWriter bw = new (File.Create (saveFile.FileName))) {
               bw.Write (dwg.ShapeList.Count);
               foreach (var drawing in dwg.ShapeList) drawing.SaveAsBin (bw);
            }
         }
         mIsDwgLoaded = true;
         mLoadCount = dwg.ShapeList.Count;
         mFileName = saveFile.FileName;
      }

      public void Save (Drawing dwg, string fileName) {
         using (BinaryWriter bw = new (File.Create (fileName))) {
            bw.Write (dwg.ShapeList.Count);
            foreach (var drawing in dwg.ShapeList) drawing.SaveAsBin (bw);
         }
         mIsDwgLoaded = true;
         mLoadCount = dwg.ShapeList.Count;
      }

      public void Open (out Drawing dwg) {
         mDrawings.Clear ();
         OpenFileDialog loadFile = new () {
            Filter = "Binary Document (*.bin)|*.bin|All (*.*)|*"
         };
         if (loadFile.ShowDialog () == true) {
            using (BinaryReader br = new (File.Open (loadFile.FileName, FileMode.Open))) {
               var shapeCount = br.ReadInt32 ();
               //Shape shape = new ();
               for (int i = 0; i < shapeCount; i++) {
                  if (mShape != null) mDrawings.Add (mShape.LoadBin (br));
               }
            }
         }
         mIsDwgLoaded = true;
         mLoadCount = mDrawings.Count;
         dwg = new ();
         for (int i = 0; i < mDrawings.Count; i++) dwg.ShapeList.Add (mDrawings[i]);
         mFileName = loadFile.FileName;
      }
      #endregion

      #region Properties --------------------------------------------
      public bool IsLoadedDwg { get => mIsDwgLoaded; set => mIsDwgLoaded = value; }

      public int LoadedDwgCount => mLoadCount;

      public string LoadedFileName => mFileName;
      #endregion

      #region Private -----------------------------------------------
      List<Shape> mDrawings = new ();
      bool mIsDwgLoaded = false; // To keep track of new file or loaded file
      int mLoadCount; // To keep track of shapes count in the loaded drawing
      string mFileName = "";
      Shape? mShape;
      #endregion
   }
}