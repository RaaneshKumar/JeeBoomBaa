using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JeeBoomBaa {
   class Items {

   }

   public class MyPoint {
      public MyPoint (double pX, double pY) {
         X = pX; Y = pY;
      }

      public double X {
         get => mX;
         set => mX = value;
      }
      double mX = double.NaN;

      public double Y {
         get => mY;
         set => mY = value;
      }
      double mY = double.NaN;
   }

   public class MyLine : MyDrawing {
      public int Rank => 1;

      public double Thickness { get; set; }

      public (MyPoint start, MyPoint end) Points = new ();
   }

   public class MyScribble : MyDrawing {
      public int Rank => 0;
      public double Thickness { get; set; }

      public List<MyPoint> Points = new ();
   }

   public class MyRect : MyDrawing {
      public int Rank => 2;

      public double Thickness { get; set; }

      public (MyPoint start, MyPoint end) Points = new ();
   }

   public class MyDrawing {
      public Brush Color { get; set; }
   }
}
