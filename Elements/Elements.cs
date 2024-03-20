using static JeeBoomBaa.EColor;

namespace JeeBoomBaa {
   #region Elements -------------------------------------------------------------------------------
   public struct MyPoint {
      #region Constructors ------------------------------------------
      public MyPoint (double pX, double pY) {
         X = pX; Y = pY;
      }
      #endregion

      #region Properties --------------------------------------------
      public double X { get; set; }

      public double Y { get; set; }
      #endregion

      #region Methods -----------------------------------------------
      public override bool Equals (object? obj) {
         if (obj == null) return false;
         if (obj is not MyPoint) return false;
         var other = (MyPoint)obj;
         if (X == other.X && Y == other.Y) return true;
         return false;
      }

      public override readonly int GetHashCode () => base.GetHashCode ();
      #endregion
   }

   public class MyDrawing {
      #region Implementation ----------------------------------------
      public EColor MyBrushColor {
         get => mBrush;
         set => mBrush = value;
      }
      EColor mBrush = White;

      public virtual int Rank => -1;

      public EShape Shape { get; set; }

      public List<MyPoint> PointList = new ();
      #endregion

      #region Methods -----------------------------------------------
      public virtual void SaveAsBin (BinaryWriter bw) {
         bw.Write (Rank);
         bw.Write (MyBrushColor.ToString ());
         bw.Write (PointList.Count);
         foreach (var point in PointList) {
            bw.Write (point.X); bw.Write (point.Y);
         }
      }

      public virtual MyDrawing LoadBin (BinaryReader br) {
         MyDrawing drawing = br.ReadInt32 () switch { // Rank
            0 => new MyScribble (),
            1 => new MyLine (),
            2 => new MyRect (),
            _ => new MyConnectedLine ()
         };
         drawing.MyBrushColor = br.ReadString () switch {
            "Red" => Red,
            "Green" => Green,
            "Yellow" => Yellow,
            _ => White
         };

         var pointCount = br.ReadInt32 ();
         for (int j = 0; j < pointCount; j++)
            drawing.PointList.Add (new (br.ReadDouble (), br.ReadDouble ()));
         return drawing;
      }
      #endregion
   }

   public enum EShape { SCRIBBLE, LINE, RECTANGLE, CONNECTEDLINE }

   public enum EColor { White, Red, Green, Yellow }
   #endregion
}