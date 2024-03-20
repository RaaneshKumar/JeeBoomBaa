using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace JeeBoomBaa {
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
      }

      public CustomCanvas MyCanvas => mCanvas;

      void Clear_Click (object sender, RoutedEventArgs e) => mCanvas.ClearDrawings ();

      void SaveAsBin_Click (object sender, RoutedEventArgs e) {
         mCanvas.DocManager.SaveFile (mCanvas.Drawings);
      }

      void LoadAsBin_Click (object sender, RoutedEventArgs e) {
         mCanvas.ClearData ();
         mCanvas.DocManager.OpenFile (out List<MyDrawing> drawings);
         for (int i = 0; i < drawings.Count; i++) mCanvas.Drawings.Add (drawings[i]);
         mCanvas.InvalidateVisual ();
      }

      void Select_Click (object sender, RoutedEventArgs e) { throw new NotImplementedException (); }

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

      void New_Click (object sender, RoutedEventArgs e) => mCanvas.New ();

      void Save_Click (object sender, RoutedEventArgs e) {
         throw new NotImplementedException ();
      }

      void Exit_Click (object sender, RoutedEventArgs e) {
         throw new NotImplementedException ();
      }
   }
}