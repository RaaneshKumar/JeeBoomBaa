using System.Collections.Generic;
using System.Windows.Controls;
using static JeeBoomBaa.EColor;
using static JeeBoomBaa.EShape;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace JeeBoomBaa {
   #region Custom Canvas --------------------------------------------------------------------------
   public partial class CustomCanvas : Canvas {
      #region Constructors ------------------------------------------
      public CustomCanvas () {
         Loaded += delegate {
            var bound = new Bound (new (-10, -10), new (1000, 1000));
            mProjXfm = Transform.ComputeZoomExtentsProjXfm (ActualWidth, ActualHeight, bound);
            mInvProjXfm = mProjXfm; mInvProjXfm.Invert ();
         };

         MouseWheel += (sender, e) => {
            double zoomFactor = 1.05;
            if (e.Delta > 0) zoomFactor = 1 / zoomFactor;
            var ptAt = mInvProjXfm.Transform (e.GetPosition (this)); // Point at which zoom is handled
            // Actual visible drawing area
            Point cornerA = mInvProjXfm.Transform (new Point (20, 20)),
                  cornerB = mInvProjXfm.Transform (new Point (ActualWidth, ActualHeight));
            var bound = new Bound (cornerA, cornerB);
            bound = bound.Inflated (new (ptAt.X, ptAt.Y), zoomFactor);
            mProjXfm = Transform.ComputeZoomExtentsProjXfm (ActualWidth, ActualHeight, bound);
            mInvProjXfm = mProjXfm; mInvProjXfm.Invert ();
            InvalidateVisual ();
         };
      }
      #endregion

      #region Properties --------------------------------------------
      public MainWindow? Window { get; set; }

      public DocManager DocManager => mDocManager;

      public Drawing Dwg => mDrawing;

      public EColor Brush => mBrush;

      public Shape Shape { get => mShape; set => mShape = value; }

      public Stack<Shape> RedoItems => mRedoItems;

      public Widget Widget { get => mWidget!; set => mWidget = value; }

      public Matrix Xfm { get => mProjXfm; set => mProjXfm = value; }

      public Matrix InvXfm { get => mInvProjXfm; set => mInvProjXfm = value; }
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
         DrawingGenerator dg = new (dc, mProjXfm);
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
      Matrix mProjXfm = Matrix.Identity, mInvProjXfm = Matrix.Identity;
      #endregion
   }
   #endregion
}