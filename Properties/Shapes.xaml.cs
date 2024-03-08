using System.Windows;

namespace JeeBoomBaa {
   /// <summary>
   /// Interaction logic for ShapeSelection.xaml
   /// </summary>
   public partial class Shapes : Window {
      public Shapes () {
         InitializeComponent ();
      }

      public CustomCanvas OwnerCanvas => ((MainWindow)Owner).MyCanvas;

      void Scribble_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.ScribbleOn ();
         Close ();
      }

      void Rectangle_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.RectOn ();
         Close ();
      }

      void Line_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.LineOn ();
         Close ();
      }

      private void ConnectedLine_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.ConnectedLineOn ();
         Close ();
      }
   }
}