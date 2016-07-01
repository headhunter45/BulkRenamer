using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace BulkRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.FolderBrowserDialog _openFolderDialog = null;
        public ObservableCollection<SearchResultItemModel> SearchResults = new ObservableCollection<SearchResultItemModel>();

        public MainWindow()
        {
            InitializeComponent();
            _openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            DataContext = this;

            tbRootFolder.Text = @"";
            tbMatch.Text = @"(.*)\.(.*)";
            tbReplace.Text = @"$1.$2";
        }

        protected System.Windows.Forms.IWin32Window GetIWin32Window()
        {
            return new MyWin32Window((System.Windows.PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource).Handle);
        }

        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = _openFolderDialog.ShowDialog(GetIWin32Window());
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbRootFolder.Text = _openFolderDialog.SelectedPath;
            }
        }

        private void tbRootFolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshSearch();
        }

        private void tbMatch_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshSearch();
        }

        private void tbReplace_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshSearch();
        }

        private void ckSelectAllRows_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var result in SearchResults)
            {
                result.IsSelected = true;
            }

            dgSearchResults.ItemsSource = null;
            dgSearchResults.ItemsSource = SearchResults;
        }

        private void ckSelectAllRows_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var result in SearchResults)
            {
                result.IsSelected = false;
            }

            dgSearchResults.ItemsSource = null;
            dgSearchResults.ItemsSource = SearchResults;
        }

        private void RefreshSearch()
        {
            String folder = tbRootFolder.Text;
            String pattern = tbMatch.Text;
            String replacement = tbReplace.Text;

            SearchResults.Clear();
            if (!String.IsNullOrWhiteSpace(folder)
                && !String.IsNullOrWhiteSpace(pattern)
                && !String.IsNullOrWhiteSpace(replacement))
            {
                try
                {
                    var regex = new Regex(pattern, RegexOptions.Compiled);
                    SearchFolder(folder, regex, replacement, false);
                    dgSearchResults.ItemsSource = SearchResults;
                    ckSelectAllRows.IsChecked = true;
                    Console.WriteLine("Search Done");
                }
                catch (ArgumentException)
                { }
            }
        }

        private void SearchFolder(String folderPath, Regex regex, String replacement, Boolean recursive)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);

            try
            {
                foreach (FileInfo fileInfo in dirInfo.EnumerateFiles())
                {
                    String before = fileInfo.Name;
                    String after;

                    if (regex.IsMatch(before))
                    {
                        after = regex.Replace(before, replacement);

                        SearchResults.Add(new SearchResultItemModel(folderPath, before, after));
                    }
                }

                if (recursive)
                {
                    foreach (DirectoryInfo dir in dirInfo.EnumerateDirectories())
                    {
                        SearchFolder(dir.FullName, regex, replacement, recursive);
                    }
                }
            }
            catch (DirectoryNotFoundException) { }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            String before;
            String after;
            foreach (var result in SearchResults)
            {
                try
                {
                    if (result.IsSelected)
                    {
                        before = String.Format(@"{0}\{1}", result.Folder, result.Before);
                        after = String.Format(@"{0}\{1}", result.Folder, result.After);
                        System.IO.File.Move(before, after);
                    }
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show(ex.Message, "Error while moving file.", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
            }

            RefreshSearch();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "cmd";
            saveDialog.CheckPathExists = true;
            saveDialog.Filter = "Batch Files|*.cmd|All Files|*.*";
            //saveDialog.Title = "";
            saveDialog.ValidateNames = true;

            var dialogResult = saveDialog.ShowDialog();
            if (dialogResult == true)
            {
                String fileToSave = saveDialog.FileName;
                StringBuilder sb = new StringBuilder();

                foreach (var result in SearchResults)
                {
                    if (result.IsSelected)
                    {
                        sb.AppendLine(String.Format(@"move ""{0}\{1}"" ""{0}\{2}""", result.Folder, result.Before, result.After));
                    }
                }

                System.IO.File.WriteAllText(fileToSave, sb.ToString());
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
