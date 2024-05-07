using System.Collections.Generic;
using System.Windows.Controls;
using static JeeBoomBaa.EColor;
using static JeeBoomBaa.EShape;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;

namespace JeeBoomBaa {
   #region Custom Canvas --------------------------------------------------------------------------
   public partial class CustomCanvas : Canvas {
      #region Properties --------------------------------------------
      public MainWindow? Window { get; set; }

      public DocManager DocManager => mDocManager;

      public Drawing Dwg => mDrawing;

      public EColor Brush => mBrush;

      public Shape Shape { get => mShape; set => mShape = value; }

      public Stack<Shape> RedoItems => mRedoItems;

      public Widget Widget { get => mWidget!; set => mWidget = value; }
      #endregion

      #region Methods -----------------------------------------------
      public void ShapeOn (EShape shape) {
         mWidget?.Detach ();
         mWidget = shape is LINE ? new LineWidget (this) : shape is RECTANGLE ? new RectWidget (this) : new ConLineWidget (this);
         mWidget.Attach ();
      }

      public void ChangeColor (EColor color) {
         mBrush = color;
         mShape.MyBrushColor = mBrush;
      }

      public void Undo () {
         var count = mDrawing.ShapeList.Count;
         if ((mDocManager.IsLoadedDwg && mDocManager.LoadedDwgCount == count) || count == 0) return;
         mRedoItems.Push (mDrawing.ShapeList[^1]);
         mDrawing.ShapeList.RemoveAt (mDrawing.ShapeList.Count - 1);
         InvalidateVisual ();
      }

      public void Redo () {
         if (mRedoItems.Count == 0) return;
         mDrawing.ShapeList.Add (mRedoItems.Pop ());
         InvalidateVisual ();
      }

      public void New () {
         ClearData ();
         mShape = new Scribble { Name = SCRIBBLE, MyBrushColor = Black };
         InvalidateVisual ();
      }

      public void ClearData () {
         mDrawing.ShapeList.Clear ();
         mRedoItems.Clear ();
         //mClearUndo.Clear ();
      }
      #endregion

      #region Implementation ----------------------------------------
      protected override void OnRender (DrawingContext dc) {
         base.OnRender (dc);
         DrawingGenerator dg = new (dc);
         for (int i = 0; i < mDrawing.ShapeList.Count; i++) dg.Draw (mDrawing);
         if (mShape.Exists) dg.Draw (mShape);
      }
      #endregion

      #region Keyboard Events ---------------------------------------
      public void KeyPressed (Key k) {
         if (mShape.Name is CONNECTEDLINE && mShape.Exists && k == Key.Escape) {
            mShape.PointList[^1] = mShape.PointList[^2];
            mDrawing.ShapeList.Add (mShape);
            mShape = new ConnectedLine { Name = CONNECTEDLINE, MyBrushColor = mBrush };
         }
         if (Window != null) Window.PromptMsg.Text = Widget.PromptMsg;
         InvalidateVisual ();
      }
      #endregion

      #region Private -----------------------------------------------
      EColor mBrush = Black;
      Shape mShape = new Scribble { Name = SCRIBBLE, MyBrushColor = Black };
      Drawing mDrawing = new ();
      Stack<Shape> mRedoItems = new ();
      DocManager mDocManager = new ();
      Widget? mWidget;
      #endregion
   }
   #endregion
}