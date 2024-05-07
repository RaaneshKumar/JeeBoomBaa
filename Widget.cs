using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static JeeBoomBaa.EShape;

namespace JeeBoomBaa {
   #region Widgets --------------------------------------------------------------------------------
   public abstract class Widget {
      #region Constructors ------------------------------------------
      public Widget (CustomCanvas canvas) {
         mCanvas = canvas;
      }
      #endregion

      #region Properties --------------------------------------------
      public abstract string PromptMsg { get; }

      public abstract string[] PromptData { get; }
      #endregion

      #region Methods -----------------------------------------------
      public void Attach () {
         mCanvas.MouseDown += OnMouseDown;
         mCanvas.MouseMove += OnMouseDrag;
      }

      public void Detach () {
         mCanvas.MouseDown -= OnMouseDown;
         mCanvas.MouseMove -= OnMouseDrag;
      }

      protected abstract void OnMouseDown (object sender, MouseEventArgs e);

      protected abstract void OnMouseDrag (object sender, MouseEventArgs e);
      #endregion

      #region Private -----------------------------------------------
      protected CustomCanvas mCanvas;
      #endregion
   }
   public class LineWidget : Widget {
      #region Constructors ------------------------------------------
      public LineWidget (CustomCanvas canvas) : base (canvas) {
         //mCanvas = canvas;
         mCanvas.Shape = new Line { Name = LINE, MyBrushColor = mCanvas.Brush };
      }
      #endregion

      #region Properties --------------------------------------------
      public override string PromptMsg
         => mCanvas.Shape.PointList.Count > 0
         ? "Line : Pick end point"
         : "Line : Pick beginning point";

      public override string[] PromptData => sPromptData;
      #endregion

      #region Methods -----------------------------------------------
      protected override void OnMouseDown (object sender, MouseEventArgs e) {
         if (mCanvas.Shape.PointList.Count == 0) mCanvas.RedoItems.Clear ();
         System.Windows.Point point = e.GetPosition (mCanvas);
         if (!mCanvas.Shape.Exists)
            mCanvas.Shape.PointList.Add (new (point.X, point.Y));
         else if (mCanvas.Shape.PointList.Count == 2) {
            mCanvas.Dwg.ShapeList.Add (mCanvas.Shape);
            mCanvas.Shape = new Line { Name = LINE, MyBrushColor = mCanvas.Brush };
         }
         if (mCanvas.Window != null) mCanvas.Window.PromptMsg.Text = PromptMsg;
         mCanvas.InvalidateVisual ();
      }

      protected override void OnMouseDrag (object sender, MouseEventArgs e) {
         System.Windows.Point point = e.GetPosition (mCanvas);
         if (mCanvas.Shape.Exists) {
            if (mCanvas.Shape.PointList.Count == 1) mCanvas.Shape.PointList.Add (new (point.X, point.Y));
            else mCanvas.Shape.PointList[^1] = new (point.X, point.Y);
            mCanvas.InvalidateVisual ();
         }
      }
      #endregion

      #region Private -----------------------------------------------
      //CustomCanvas mCanvas;
      static string[] sPromptData = { "X", "Y", "Length" };
      #endregion
   }

   public class RectWidget : Widget {
      #region Constructors ------------------------------------------
      public RectWidget (CustomCanvas canvas) : base (canvas) {
         //mCanvas = canvas;
         mCanvas.Shape = new Rectangle { Name = RECTANGLE, MyBrushColor = mCanvas.Brush };
      }
      #endregion

      #region Properties --------------------------------------------
      public override string PromptMsg
         => mCanvas.Shape.PointList.Count > 0
         ? "Rect : Pick opposite corner of the rectangle"
         : "Rect : Pick first corner of the rectangle";


      public override string[] PromptData => sPromptData;
      #endregion

      #region Methods -----------------------------------------------
      protected override void OnMouseDown (object sender, MouseEventArgs e) {
         if (mCanvas.Shape.PointList.Count == 0) mCanvas.RedoItems.Clear ();
         System.Windows.Point point = e.GetPosition (mCanvas);
         if (!mCanvas.Shape.Exists)
            mCanvas.Shape.PointList.Add (new (point.X, point.Y));
         else if (mCanvas.Shape.PointList.Count == 2) {
            mCanvas.Dwg.ShapeList.Add (mCanvas.Shape);
            mCanvas.Shape = new Rectangle { Name = RECTANGLE, MyBrushColor = mCanvas.Brush };
         }
         if (mCanvas.Window != null) mCanvas.Window.PromptMsg.Text = PromptMsg;
         mCanvas.InvalidateVisual ();
      }

      protected override void OnMouseDrag (object sender, MouseEventArgs e) {
         System.Windows.Point point = e.GetPosition (mCanvas);
         if (mCanvas.Shape.Exists) {
            if (mCanvas.Shape.PointList.Count == 1) mCanvas.Shape.PointList.Add (new (point.X, point.Y));
            else mCanvas.Shape.PointList[^1] = new (point.X, point.Y);
            mCanvas.InvalidateVisual ();
         }
      }
      #endregion

      #region Private -----------------------------------------------
      //CustomCanvas mCanvas;
      static string[] sPromptData = { "X", "Y", "Width", "Height" };
      #endregion
   }

   public class ConLineWidget : Widget {
      #region Constructors ------------------------------------------
      public ConLineWidget (CustomCanvas canvas) : base (canvas) {
         //mCanvas = canvas;
         mCanvas.Shape = new ConnectedLine { Name = CONNECTEDLINE, MyBrushColor = mCanvas.Brush };
      }
      #endregion

      #region Properties --------------------------------------------
      public override string PromptMsg
         => mCanvas.Shape.PointList.Count > 0
         ? "Connected Line : Click on next point (Click \"Esc\" to end connected line)"
         : "Connected Line : Click on start point";

      public override string[] PromptData => sPromptData;
      #endregion

      #region Methods -----------------------------------------------
      protected override void OnMouseDown (object sender, MouseEventArgs e) {
         if (mCanvas.Shape.PointList.Count == 0) mCanvas.RedoItems.Clear ();
         System.Windows.Point point = e.GetPosition (mCanvas);
         mCanvas.Shape.PointList.Add (new (point.X, point.Y));
         if (mCanvas.Window != null) mCanvas.Window.PromptMsg.Text = PromptMsg;
         mCanvas.InvalidateVisual ();
      }

      protected override void OnMouseDrag (object sender, MouseEventArgs e) {
         System.Windows.Point point = e.GetPosition (mCanvas);
         if (mCanvas.Shape.Exists) {
            if (mCanvas.Shape.PointList.Count == 1) mCanvas.Shape.PointList.Add (new (point.X, point.Y));
            else mCanvas.Shape.PointList[^1] = new (point.X, point.Y);
            mCanvas.InvalidateVisual ();
         }
      }
      #endregion

      #region Private -----------------------------------------------
      //CustomCanvas mCanvas;
      static readonly string[] sPromptData = { "X", "Y", "Length" };
      #endregion
   }
   #endregion
}