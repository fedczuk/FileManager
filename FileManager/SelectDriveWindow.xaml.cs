using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for SelectDriveWindow.xaml
    /// </summary>
    public partial class SelectDriveWindow : Window
    {
        public SelectDriveWindow(Window owner)
        {
            InitializeComponent();
            this.Owner = owner;
            this.lvDrives.Focus();

            DriveInfo[] drives = DriveInfo.GetDrives();
            this.lvDrives.ItemsSource = drives;
        }

        private void lvDrivesKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.bOKClickEvent(null, null);
        }

        private void bOKClickEvent(object sender, RoutedEventArgs e)
        {
            if (this.lvDrives.SelectedIndex >= 0)
                this.DialogResult = true;
        }
    }
}
