using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
      public CustomCanvas MyCanvas => mCanvas;


      void Clear_Click (object sender, RoutedEventArgs e) => mCanvas.ClearPoints ();
      void SaveAsText_Click (object sender, RoutedEventArgs e) => mCanvas.SaveAsText ();
      void SaveAsBin_Click (object sender, RoutedEventArgs e) {
         mCanvas.SaveAsBin ();
      }
      void LoadAsText_Click (object sender, RoutedEventArgs e) => mCanvas.LoadAsText ();
      private void LoadAsBin_Click (object sender, RoutedEventArgs e) {
         mCanvas.LoadAsBin ();
      }
      void Eraser_Click (object sender, RoutedEventArgs e) => mCanvas.EnableEraser ();

      void Colors_Click (object sender, RoutedEventArgs e) {
         ColorSelection colorSelection = new () {Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
         colorSelection.Show ();
      }
      void Undo_Click (object sender, RoutedEventArgs e) => mCanvas.Undo ();
      void Redo_Click (object sender, RoutedEventArgs e) => mCanvas.Redo ();
   }
}