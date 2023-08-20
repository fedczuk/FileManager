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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics; 

namespace FileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListView activeList;
        public static RoutedCommand ChangeFileList = new RoutedCommand("ChangeFileList", typeof(MainWindow));

        public MainWindow()
        {
            Commands.Initialize();
            InitializeComponent();

            string dir1, dir2;
            if ((bool)Properties.Settings.Default.RememberOpenedDir == true)
            {
                dir1 = Properties.Settings.Default.Dir1;
                dir2 = Properties.Settings.Default.Dir2;
            }
            else
            {
                dir1 = dir2 = System.IO.Path.GetPathRoot(Environment.SystemDirectory); 
            }
            this.ViewFoldersAndFiles(this.lvFile1, dir1);
            this.ViewFoldersAndFiles(this.lvFile2, dir2);

            this.activeList = this.lvFile1;
            this.activeList.Focus();
        }

        private bool ViewFoldersAndFiles(ListView lv, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                lv.Items.Clear();
                lv.Tag = path;

                foreach (string dir in dirs)
                    lv.Items.Add(new DInfo(new DirectoryInfo(dir)));
                
                foreach (string file in files)
                    lv.Items.Add(new FInfo(new FileInfo(file)));

                this.PathUpdate(lv);
                if (lv.Items.Count > 0)
                    lv.ScrollIntoView(lv.Items[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private void ExecuteItem(object sender)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedIndex < 0)
                return;

            IDiskEntry entryInfo = lv.SelectedItem as IDiskEntry;
            string path = System.IO.Path.Combine((lv.Tag as string), entryInfo.Name);

            if (entryInfo.Extension.Equals("<DIR>"))
                this.ViewFoldersAndFiles(lv, path);
            else
                System.Diagnostics.Process.Start(path);
        }

        private void GoLevelUp(object sender)
        {
            ListView lv = sender as ListView;
            DirectoryInfo path = new DirectoryInfo(lv.Tag as string);
            try
            {
                this.ViewFoldersAndFiles(lv, path.Parent.FullName);
            }
            catch (NullReferenceException) { }
        }

        private void RefreshFileList()
        {
            string di1 = this.lvFile1.Tag as string;
            string di2 = this.lvFile2.Tag as string;

            if (di1.Equals(di2))
            {
                this.ViewFoldersAndFiles(this.lvFile1, di1);
                this.ViewFoldersAndFiles(this.lvFile2, di2);
            }
            else
            {
                string activeDir = this.activeList.Tag as string;
                this.ViewFoldersAndFiles(this.activeList, activeDir);
            }
        }

        private void PathUpdate(object sender)
        {
            ListView lv = sender as ListView;
            lPath.Content = lv.Tag as string;
            lCount.Content = "Liczba elementów: " + lv.Items.Count;
        }

        private void lvFileMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.ExecuteItem(sender);
        }

        private void ChangeFileListCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.activeList.Equals(this.lvFile2))
                this.lvFile1.Focus();
            else
                this.lvFile2.Focus();
        }

        private void lvFileGotFocus(object sender, RoutedEventArgs e)
        {
            ListView lv = sender as ListView;
            this.activeList = lv;
            this.PathUpdate(this.activeList);
        }

        private void SettingsClickEvent(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow(this);

            sw.cbRunWithSystem.IsChecked = Properties.Settings.Default.RunWithSystem;
            sw.cbRememberOpenedDir.IsChecked = Properties.Settings.Default.RememberOpenedDir;

            sw.ShowDialog();
            if (sw.DialogResult.HasValue && sw.DialogResult.Value)
            {
                Properties.Settings.Default.RunWithSystem = (bool)sw.cbRunWithSystem.IsChecked;
                Properties.Settings.Default.RememberOpenedDir = (bool)sw.cbRememberOpenedDir.IsChecked;

                Properties.Settings.Default.Save();
            }
        }

        private void ExitClickEvent(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GoFsLevelUpCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.GoLevelUp(this.activeList);
        }

        private void GoToCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Confirm cd = new Confirm(this);
            cd.Title = "Idź do";
            cd.tbInputText.KeyDown += new KeyEventHandler(GoTotbInputTextKeyDown);
            cd.tbInputText.TextChanged += new TextChangedEventHandler(tbInputText_TextChanged);
            cd.ShowDialog();
            if (cd.DialogResult.HasValue && cd.DialogResult.Value)
            {
                this.ViewFoldersAndFiles(this.activeList, cd.tbInputText.Text);
            }
        }

        void tbInputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            
            if (tb.Tag == null)
                return;

            tb.Tag = null;

            int lastBackSlash = tb.Text.LastIndexOf('\\');

            if (lastBackSlash >= 0)
            {
                string searchPath = tb.Text.Substring(0, lastBackSlash + 1);
                string searchName = tb.Text.Substring(lastBackSlash + 1);
                if (!String.IsNullOrEmpty(searchName))
                {
                    try
                    {
                        string[] dirs = Directory.GetDirectories(searchPath, searchName + "*");
                        if (dirs.Length > 0)
                        {
                            int tbLength = tb.Text.Length;
                            tb.Text = dirs[0];
                            tb.Select(tbLength, tb.Text.Length - tbLength);
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }

        void GoTotbInputTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                return;

            TextBox tb = sender as TextBox;
            if (e.Key == Key.LeftCtrl && tb.SelectionLength > 0)
            {
                tb.SelectionStart = tb.Text.Length;
                tb.AppendText("\\");
            }
            tb.Tag = true;
        }

        private void NewDirCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Confirm cf = new Confirm(this);
            cf.Title = "Nowy folder";
            cf.ShowDialog();

            if (cf.DialogResult.HasValue && cf.DialogResult.Value)
            {
                string path = this.activeList.Tag as string;
                FileSystem.CreateDirectory(System.IO.Path.Combine(path, cf.tbInputText.Text));
                this.RefreshFileList();
            }
            this.activeList.Focus();
        }

        private void NewFileCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Confirm cf = new Confirm(this);
            cf.Title = "Nowy plik";
            cf.ShowDialog();

            if (cf.DialogResult.HasValue && cf.DialogResult.Value)
            {
                string path = this.activeList.Tag as string;
                int dot = cf.tbInputText.Text.LastIndexOf('.');
                string filename = cf.tbInputText.Text;
                if (dot < 0)
                    filename += ".txt";
                File.Create(System.IO.Path.Combine(path, filename));
                this.RefreshFileList();
            }
            this.activeList.Focus();
        }

        private void ListViewCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.activeList != null && this.activeList.SelectedIndex >= 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void ChangeNameCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IDiskEntry diskEntry = this.activeList.SelectedItem as IDiskEntry;
            Confirm cf = new Confirm(this);
            cf.Title = "Zmiana nazwy";
            cf.tbInputText.Text = diskEntry.Name;
            cf.ShowDialog();

            if (cf.DialogResult.HasValue && cf.DialogResult.Value)
            {
                string path = System.IO.Path.Combine((this.activeList.Tag as string), diskEntry.Name);
                if (diskEntry.Extension.Equals("<DIR>"))
                    FileSystem.RenameDirectory(path, cf.tbInputText.Text);
                else
                    FileSystem.RenameFile(path, cf.tbInputText.Text);
                this.RefreshFileList();
            }
            this.activeList.Focus();
        }

        private void CopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ListView destView;
            if (this.activeList.Equals(this.lvFile2))
                destView = this.lvFile1;
            else
                destView = this.lvFile2;

            IDiskEntry source = this.activeList.SelectedItem as IDiskEntry;
            string sourcePath = this.activeList.Tag as string;
            string destination = destView.Tag as string;

            try
            {
                if (source.Extension.Equals("<DIR>"))
                    FileSystem.CopyDirectory(System.IO.Path.Combine(sourcePath, source.Name), System.IO.Path.Combine(destination, source.Name), UIOption.AllDialogs);
                else
                    FileSystem.CopyFile(System.IO.Path.Combine(sourcePath, source.Name), System.IO.Path.Combine(destination, source.Name), UIOption.AllDialogs);

                this.ViewFoldersAndFiles(destView, destination);
            }
            catch (Exception) { }
            this.activeList.Focus();
        }

        private void MoveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ListView destView;
            if (this.activeList.Equals(this.lvFile2))
                destView = this.lvFile1;
            else
                destView = this.lvFile2;

            IDiskEntry source = this.activeList.SelectedItem as IDiskEntry;
            string sourcePath = this.activeList.Tag as string;
            string destination = destView.Tag as string;

            try
            {
                if (source.Extension.Equals("<DIR>"))
                    FileSystem.MoveDirectory(System.IO.Path.Combine(sourcePath, source.Name), System.IO.Path.Combine(destination, source.Name), UIOption.AllDialogs);
                else
                    FileSystem.MoveFile(System.IO.Path.Combine(sourcePath, source.Name), System.IO.Path.Combine(destination, source.Name), UIOption.AllDialogs);

                this.ViewFoldersAndFiles(this.activeList, sourcePath);
                this.ViewFoldersAndFiles(destView, destination);
            }
            catch (Exception) { }
            this.activeList.Focus();
        }

        private void DeleteCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IDiskEntry source = this.activeList.SelectedItem as IDiskEntry;
            string path = this.activeList.Tag as string;
            try
            {
                if (source.Extension.Equals("<DIR>"))
                    FileSystem.DeleteDirectory(System.IO.Path.Combine(path, source.Name), UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                else
                    FileSystem.DeleteFile(System.IO.Path.Combine(path, source.Name), UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception) { }
            this.RefreshFileList();
        }

        private void RunCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.ExecuteItem(this.activeList);
        }

        private void ConsoleCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string path = this.activeList.Tag as string;
            Process.Start("cmd.exe", "/K " + "cd \"" + path + "\"");
        }

        private void FastSearchCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Confirm cf = new Confirm(this);
            cf.Title = "Szybkie wyszukiwanie";
            cf.tbInputText.TextChanged += new TextChangedEventHandler(FastSearch_tbInputTextChangedEvent);
            cf.ShowDialog();
            this.activeList.Focus();
        }

        void FastSearch_tbInputTextChangedEvent(object sender, TextChangedEventArgs e)
        {
            TextBox tbInput = sender as TextBox;
            for(int i = 0; i < this.activeList.Items.Count; i++)
            {
                IDiskEntry fdi = this.activeList.Items[i] as IDiskEntry;
                if (fdi.Name.ToLower().StartsWith(tbInput.Text.ToLower()))
                {
                    this.activeList.SelectedIndex = i;
                    this.activeList.ScrollIntoView(fdi);
                    break;
                }
            }
        }

        private void SelectDriveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectDriveWindow sdw = new SelectDriveWindow(this);
            sdw.ShowDialog();
            if (sdw.DialogResult.HasValue && sdw.DialogResult.Value)
            {
                DriveInfo di = sdw.lvDrives.SelectedItem as DriveInfo;
                this.ViewFoldersAndFiles(this.activeList, di.RootDirectory.FullName);
            }
        }

        private void HomeDirCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewFoldersAndFiles(this.activeList, Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        }

        private void HelpCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (File.Exists("pomoc.rtf"))
                System.Diagnostics.Process.Start("pomoc.rtf");
            else
                MessageBox.Show("Brak pliku pomocy.");
        }

        private void AboutClickEvent(object sender, RoutedEventArgs e)
        {
            AboutWindow aw = new AboutWindow();
            aw.Owner = this;
            aw.ShowDialog();
        }

        private void MainWindowClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((bool)Properties.Settings.Default.RememberOpenedDir == true)
            {
                string dir1 = lvFile1.Tag as string;
                string dir2 = lvFile2.Tag as string;

                Properties.Settings.Default.Dir1 = dir1;
                Properties.Settings.Default.Dir2 = dir2;
                Properties.Settings.Default.Save();
            }
        }
    }
}
