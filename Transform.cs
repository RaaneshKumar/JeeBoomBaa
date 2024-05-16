using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace JeeBoomBaa {
   class Transform {
      public static Matrix ComputeZoomExtentsProjXfm (double viewWidth, double viewHeight, Bound bound) {
         var viewMargin = 20;
         double scaleX = (viewWidth - 2 * viewMargin) / bound.Width,
                scaleY = (viewHeight - 2 * viewMargin) / bound.Height;
         double scale = Math.Min (scaleX, scaleY);
         var scaleMatrix = Matrix.Identity; scaleMatrix.Scale (scale, -scale);
         Point projMidPt = scaleMatrix.Transform (bound.Mid);
         Point viewMidPt = new (viewWidth / 2, viewHeight / 2);
         var translateMatrix = Matrix.Identity;
         translateMatrix.Translate (viewMidPt.X - projMidPt.X, viewMidPt.Y - projMidPt.Y);
         scaleMatrix.Append (translateMatrix); // Final zoom extents matrix
         return scaleMatrix;
      }
   }

   readonly struct Bound { // Bound in drawing space
      #region Constructors
      public Bound (Point cornerA, Point cornerB) {
         MinX = Math.Min (cornerA.X, cornerB.X);
         MaxX = Math.Max (cornerA.X, cornerB.X);
         MinY = Math.Min (cornerA.Y, cornerB.Y);
         MaxY = Math.Max (cornerA.Y, cornerB.Y);
      }

      public Bound () => this = Empty;

      public static readonly Bound Empty = new () {
         MinX = double.MaxValue, MinY = double.MaxValue, MaxX = double.MinValue, MaxY = double.MinValue
      };
      #endregion

      #region Properties
      public double MinX { get; init; }

      public double MaxX { get; init; }

      public double MinY { get; init; }

      public double MaxY { get; init; }

      public double Width => MaxX - MinX;

      public double Height => MaxY - MinY;

      public Point Mid => new ((MaxX + MinX) / 2, (MaxY + MinY) / 2);

      public bool IsEmpty => MinX > MaxX || MinY > MaxY;
      #endregion

      #region Methods
      public Bound Inflated (Point ptAt, double factor) {
         if (IsEmpty) return this;
         var minX = ptAt.X - (ptAt.X - MinX) * factor;
         var maxX = ptAt.X + (MaxX - ptAt.X) * factor;
         var minY = ptAt.Y - (ptAt.Y - MinY) * factor;
         var maxY = ptAt.Y + (MaxY - ptAt.Y) * factor;
         return new () { MinX = minX, MaxX = maxX, MinY = minY, MaxY = maxY };
      }
      #endregion
   }
}