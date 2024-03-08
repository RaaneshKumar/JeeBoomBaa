using System.Windows;
using System.Windows.Input;

namespace JeeBoomBaa {
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
      }

      public CustomCanvas MyCanvas => mCanvas;

      void Clear_Click (object sender, RoutedEventArgs e) => mCanvas.ClearPoints ();

      void SaveAsText_Click (object sender, RoutedEventArgs e) => mCanvas.SaveAsText ();

      void SaveAsBin_Click (object sender, RoutedEventArgs e) => mCanvas.SaveAsBin ();

      void LoadAsText_Click (object sender, RoutedEventArgs e) => mCanvas.LoadAsText ();

      void LoadAsBin_Click (object sender, RoutedEventArgs e) => mCanvas.LoadAsBin ();

      void Select_Click (object sender, RoutedEventArgs e) { }

      void Colors_Click (object sender, RoutedEventArgs e) {
         ColorSelection colorSelection = new () { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
         colorSelection.Show ();
      }

      void Undo_Click (object sender, RoutedEventArgs e) => mCanvas.Undo ();

      void Redo_Click (object sender, RoutedEventArgs e) => mCanvas.Redo ();

      void Shapes_Click (object sender, RoutedEventArgs e) {
         Shapes shapeSelection = new () { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
         shapeSelection.Show ();
      }

      protected override void OnKeyDown (KeyEventArgs e) => mCanvas.KeyPressed (e.Key);

      void New_Click (object sender, RoutedEventArgs e) {
         MainWindow mainWindow2 = new () { Owner = this, WindowStartupLocation = WindowStartupLocation.Manual };
         mainWindow2.Show ();
      }

      void Save_Click (object sender, RoutedEventArgs e) {

      }

      void Exit_Click (object sender, RoutedEventArgs e) {

      }
   }
}