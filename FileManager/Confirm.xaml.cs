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

namespace FileManager
{
    /// <summary>
    /// Interaction logic for Confirm.xaml
    /// </summary>
    public partial class Confirm : Window
    {
        public Confirm(Window owner)
        {
            InitializeComponent();
            this.Owner = owner;
            this.tbInputText.Focus();
        }

        private bool Validation()
        {
            if (String.IsNullOrEmpty(this.tbInputText.Text))
                return false;
            return true;
        }

        private void tbInputTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.bOKClickEvent(null, null);
        }

        private void bOKClickEvent(object sender, RoutedEventArgs e)
        {
            if (this.Validation())
                this.DialogResult = true;
            else
            {
                this.lMessage.Visibility = Visibility.Visible;
                this.tbInputText.Focus();
            }
        }
    }
}
