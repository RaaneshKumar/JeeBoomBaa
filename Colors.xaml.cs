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
         OwnerCanvas.EnableColor (Red);
         Close ();
      }

      void Green_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.EnableColor (Green);
         Close ();
      }

      void Yellow_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.EnableColor (Yellow);
         Close ();
      }

      void White_Click (object sender, RoutedEventArgs e) {
         OwnerCanvas.EnableColor (White);
         Close ();
      }
   }
}