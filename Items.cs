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
      public MyPoint(double pX, double pY) { 
         X = pX; Y = pY;
      }

      public double X {
         get => mX;
         set => mX = value;
      }
      double mX;

      public double Y {
         get => mY;
         set => mY = value;
      }
      double mY;
   }
   public class MyLine : MyDrawing {
      public Brush Color { get; set; }
      public double Thickness { get; set; }

      public List<MyPoint> Points = new ();
   }

   public class MyDrawing {

   }
}
