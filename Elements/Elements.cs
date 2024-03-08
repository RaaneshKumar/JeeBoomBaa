using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace JeeBoomBaa {
   public struct MyPoint {
      public MyPoint (double pX, double pY) {
         X = pX; Y = pY;
      }

      public double X { get; set; }

      public double Y { get; set; }

      public override bool Equals (object? obj) {
         if (obj == null) return false;
         if (obj is not MyPoint) return false;
         var other = (MyPoint)obj;
         if (X == other.X && Y == other.Y) return true;
         return false;
      }

      public override int GetHashCode () => base.GetHashCode ();
   }

   public class MyDrawing {
      public Brush MyBrush {
         get => mBrush;
         set => mBrush = value;
      }
      Brush mBrush = Brushes.White;

      public virtual int Rank => -1;

      public virtual void Draw (DrawingContext dc) { }

      public virtual void SaveAsText (StreamWriter sw) {
         sw.WriteLine ($"{Shape}\n{MyBrush}");
         foreach (var point in PointList) sw.WriteLine ($"{point.X},{point.Y}");
         sw.WriteLine ();
      }

      public virtual void SaveAsBin (BinaryWriter bw) {
         SolidColorBrush sb = (SolidColorBrush)MyBrush;
         bw.Write (sb.Color.A); bw.Write (sb.Color.R);
         bw.Write (sb.Color.G); bw.Write (sb.Color.B);
         bw.Write (Rank);
         bw.Write (PointList.Count);
         foreach (var point in PointList) {
            bw.Write (point.X); bw.Write (point.Y);
         }
      }

      public virtual MyDrawing LoadBin (BinaryReader br) {
         byte a = br.ReadByte (), r = br.ReadByte (),
              g = br.ReadByte (), b = br.ReadByte ();
         MyDrawing drawing = br.ReadInt32 () switch { // Rank
            0 => new MyScribble (),
            1 => new MyLine (),
            2 => new MyRect (),
            _ => new MyConnectedLine ()
         };
         drawing.MyBrush = new SolidColorBrush (Color.FromArgb (a, r, g, b));
         var pointCount = br.ReadInt32 ();
         for (int j = 0; j < pointCount; j++)
            drawing.PointList.Add (new (br.ReadDouble (), br.ReadDouble ()));
         return drawing;
      }

      public EShape Shape { get; set; }

      public List<MyPoint> PointList = new ();
   }

   public enum EShape { SCRIBBLE, LINE, RECTANGLE, CONNECTEDLINE }

   public enum EColor { White, Red, Green, Yellow }
}