using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using System.Security.Policy;
using static JeeBoomBaa.EColor;
using static JeeBoomBaa.EShape;

namespace JeeBoomBaa {
   #region MainWindow -----------------------------------------------------------------------------
   public partial class MainWindow : Window {
      #region Constructors ------------------------------------------
      public MainWindow () {
         InitializeComponent ();
         mCanvas.Window = this;
         Closing += OnMainWindowClosing;
      }
      #endregion

      #region Properties --------------------------------------------
      public CustomCanvas Canvas => mCanvas;

      public TextBlock PromptMsg => mPromptMsg;

      public StackPanel PromptData => mPromptData;
      #endregion

      #region Methods -----------------------------------------------
      void OnMainWindowClosing (object? sender, CancelEventArgs e) {
         mIsModifiedDwg = mCanvas.DocManager.IsLoadedDwg
                        ? (mCanvas.Dwg.ShapeList.Count > mCanvas.DocManager.LoadedDwgCount)
                        : (mCanvas.Dwg.ShapeList.Count > 0);

         if (mIsModifiedDwg) {
            CloseWindow closeWindow = new (e) { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            closeWindow.ShowDialog ();
         }
      }

      void OnSaveAsClicked (object sender, RoutedEventArgs e) => mCanvas.DocManager.SaveAs (mCanvas.Dwg);

      void OnLoadClicked (object sender, RoutedEventArgs e) {
         mCanvas.ClearData ();
         mCanvas.DocManager.Open (out Drawing dwg);
         for (int i = 0; i < dwg.ShapeList.Count; i++) mCanvas.Dwg.ShapeList.Add (dwg.ShapeList[i]);
         mCanvas.InvalidateVisual ();
      }

      void OnSelectClicked (object sender, RoutedEventArgs e) { throw new NotImplementedException (); }

      void OnUndoClicked (object sender, RoutedEventArgs e) => mCanvas.Undo ();

      void OnRedoClicked (object sender, RoutedEventArgs e) => mCanvas.Redo ();

      protected override void OnKeyDown (KeyEventArgs e) => mCanvas.KeyPressed (e.Key);

      void OnNewClicked (object sender, RoutedEventArgs e) => mCanvas.New ();

      void OnSaveClicked (object sender, RoutedEventArgs e) {
         if (!mCanvas.DocManager.IsLoadedDwg)
            mCanvas.DocManager.SaveAs (mCanvas.Dwg);
         else
            mCanvas.DocManager.Save (mCanvas.Dwg, mCanvas.DocManager.LoadedFileName);
      }

      void OnExitClicked (object sender, RoutedEventArgs e) {
         Close ();
      }

      void OnShapeClicked (object sender, RoutedEventArgs e) {
         string btnName = "";
         if (sender is Button btn) btnName = btn.Name;
         mPromptData.Children.Clear ();
         switch (btnName) {
            case "Line": mCanvas.ShapeOn (LINE); break;
            case "Rectangle": mCanvas.ShapeOn (RECTANGLE); break;
            default: mCanvas.ShapeOn (CONNECTEDLINE); break;
         }
         foreach (var x in mCanvas.Widget.PromptData) {
            mText = new () { Text = x, FontSize = 15, Margin = new Thickness (5, 0, 0, 0) };
            mBox = new () { Width = 50, Margin = new Thickness (5, 0, 0, 0) };
            mPromptData.Children.Add (mText);
            mPromptData.Children.Add (mBox);
         }
         mPromptMsg.Text = mCanvas.Widget.PromptMsg;
      }

      void OnRedClicked (object sender, RoutedEventArgs e) => mCanvas.ChangeColor (Red);

      void OnGreenClicked (object sender, RoutedEventArgs e) => mCanvas.ChangeColor (Green);

      void OnYellowClicked (object sender, RoutedEventArgs e) => mCanvas.ChangeColor (Yellow);

      void OnBlackClicked (object sender, RoutedEventArgs e) => mCanvas.ChangeColor (Black);
      #endregion

      #region Private -----------------------------------------------
      TextBlock? mText;
      TextBox? mBox;
      bool mIsModifiedDwg = false;
      #endregion
   }
   #endregion
}