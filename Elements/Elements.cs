using static JeeBoomBaa.EColor;

namespace JeeBoomBaa {
   #region Structs --------------------------------------------------------------------------------
   public struct PointLite {
      #region Constructors ------------------------------------------
      public PointLite (double pX, double pY) {
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
         if (obj is not PointLite) return false;
         var other = (PointLite)obj;
         if (X == other.X && Y == other.Y) return true;
         return false;
      }

      public override readonly int GetHashCode () => base.GetHashCode ();
      #endregion
   }
   #endregion

   #region Interfaces -----------------------------------------------------------------------------
   public interface IDrawable {
      void DrawLine (Line line);

      void DrawRect (Rectangle rect);

      void DrawConLine (ConnectedLine conLine);
   }
   #endregion

   #region Classes --------------------------------------------------------------------------------
   public class Drawing {
      public List<Shape> ShapeList = new ();
   }

   public abstract class Shape {
      #region Implementation ----------------------------------------
      public EColor MyBrushColor {
         get => mBrush;
         set => mBrush = value;
      }
      EColor mBrush = Black;

      public EShape Name { get; set; }

      public List<PointLite> PointList = new ();

      public bool Exists => PointList.Count > 0;
      #endregion

      #region Methods -----------------------------------------------
      public abstract void Draw (IDrawable shape);

      public virtual void SaveAsBin (BinaryWriter bw) {
         bw.Write (Name.ToString ());
         bw.Write (MyBrushColor.ToString ());
         bw.Write (PointList.Count);
         foreach (var point in PointList) {
            bw.Write (point.X); bw.Write (point.Y);
         }
      }

      public virtual Shape LoadBin (BinaryReader br) {
         Shape shape = br.ReadString () switch { // Rank
            "SCRIBBLE" => new Scribble (),
            "LINE" => new Line (),
            "RECTANGLE" => new Rectangle (),
            _ => new ConnectedLine ()
         };
         shape.MyBrushColor = br.ReadString () switch {
            "Red" => Red,
            "Green" => Green,
            "Yellow" => Yellow,
            _ => Black
         };

         var pointCount = br.ReadInt32 ();
         for (int j = 0; j < pointCount; j++)
            shape.PointList.Add (new (br.ReadDouble (), br.ReadDouble ()));
         return shape;
      }
      #endregion
   }
   #endregion

   #region Enums ----------------------------------------------------------------------------------
   public enum EShape { SCRIBBLE, LINE, RECTANGLE, CONNECTEDLINE }

   public enum EColor { Black, Red, Green, Yellow }
   #endregion
}