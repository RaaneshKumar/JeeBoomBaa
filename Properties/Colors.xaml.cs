using System.Windows;
using static JeeBoomBaa.EColor;

namespace JeeBoomBaa {
   /// <summary>
   /// Interaction logic for Window1.xaml
   /// </summary>
   public partial class ColorSelection : Window {
      public ColorSelection () {
         InitializeComponent ();
      }

      public CustomCanvas OwnerCanvas => ((MainWindow)Owner).MyCanvas;

      void Red_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.ChangeColor (Red);
         Close ();
      }

      void Green_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.ChangeColor (Green);
         Close ();
      }

      void Yellow_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.ChangeColor (Yellow);
         Close ();
      }

      void White_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.ChangeColor (White);
         Close ();
      }
   }
}