using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JeeBoomBaa {
   public partial class MainWindow : Window {
      public MainWindow () => InitializeComponent ();
      void Clear_Click (object sender, RoutedEventArgs e) => mCanvas.ClearPoints ();
      void Undo_Click (object sender, RoutedEventArgs e) => mCanvas.Undo ();
      void Redo_Click (object sender, RoutedEventArgs e) => mCanvas.Redo ();
      void Save_Click (object sender, RoutedEventArgs e) {

      }
      void Eraser_Click (object sender, RoutedEventArgs e) => mCanvas.EnableEraser ();

      public CustomCanvas MyCanvas => mCanvas;

      void Colors_Click (object sender, RoutedEventArgs e) {
         ColorSelection colorSelection = new () {Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
         colorSelection.Show ();
      }
   }
}