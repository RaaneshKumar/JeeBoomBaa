using System;
using System.Collections.Generic;
using System.ComponentModel;
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
   #region CloseWindow ----------------------------------------------------------------------------
   /// <summary>
   /// Interaction logic for CloseWindow.xaml
   /// </summary>
   public partial class CloseWindow : Window {
      #region Constructors ------------------------------------------
      public CloseWindow (CancelEventArgs e) {
         InitializeComponent ();
         eMainWindow = e;
         Closing += OnCloseWindowClosing;
      }
      #endregion

      #region Properties --------------------------------------------
      public CancelEventArgs EMainWindow => eMainWindow;

      public CustomCanvas OwnerCanvas => ((MainWindow)Owner).Canvas;

      DocManager OwnerDocManager => OwnerCanvas.DocManager;
      #endregion

      #region Methods -----------------------------------------------
      void OnCloseWindowClosing (object? sender, CancelEventArgs e) {
         //EMainWindow.Cancel = true;
      }

      void OnCancelClicked (object sender, RoutedEventArgs e) {
         EMainWindow.Cancel = true;
         Close ();
      }

      void OnDontSaveClicked (object sender, RoutedEventArgs e) {
         Close ();
      }

      void OnSaveClicked (object sender, RoutedEventArgs e) {
         if (!OwnerDocManager.IsLoadedDwg) OwnerDocManager.SaveAs (OwnerCanvas.Dwg);
         else {
            OwnerDocManager.Save (OwnerCanvas.Dwg, OwnerDocManager.LoadedFileName);
         }
         Close ();
      }
      #endregion

      #region Private -----------------------------------------------
      CancelEventArgs eMainWindow;
      #endregion
   }
   #endregion
}