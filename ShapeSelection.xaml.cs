using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JeeBoomBaa {
   /// <summary>
   /// Interaction logic for ShapeSelection.xaml
   /// </summary>
   public partial class ShapeSelection : Window {
      public ShapeSelection () {
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
