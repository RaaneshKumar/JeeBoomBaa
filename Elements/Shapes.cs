namespace JeeBoomBaa {
   public class MyScribble : MyDrawing {
      public override int Rank => 0;
      public double Thickness { get; set; }
   }

   public class MyRect : MyDrawing {
      public override int Rank => 2;

      public double Thickness { get; set; }
   }

   public class MyLine : MyDrawing {
      public override int Rank => 1;

      public double Thickness { get; set; }
   }

   public class MyConnectedLine : MyDrawing {
      public override int Rank => 3;
      public double Thickness { get; set; }
   }
}