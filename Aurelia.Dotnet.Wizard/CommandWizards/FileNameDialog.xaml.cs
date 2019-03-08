using System;
using System.Collections.Generic;
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

namespace Aurelia.DotNet.Wizard.CommandWizards
{
    /// <summary>
    /// Interaction logic for FileNameDialog.xaml
    /// </summary>
    public partial class FileNameDialog : Window
    {
        public string PreviewText { get; set; }

        public string FolderLabel
        {
            set
            {
                this.lblFolder.Content = value;
            }
        }


        public FileNameDialog() { }

        public FileNameDialog(string folder = null)
        {
            InitializeComponent();
            lblFolder.Content = $"{folder}/";
            this.Loaded += FileNameDialog_Loaded;
        }

        private void FileNameDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Add Aurelia File";
            txtName.Focus();
            txtName.CaretIndex = 0;
            txtName.Text = PreviewText;
            txtName.Select(0, txtName.Text.Length);

            txtName.PreviewKeyDown += (a, b) =>
            {
                if (b.Key == Key.Escape)
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text) || txtName.Text == PreviewText)
                        Close();
                    else
                        txtName.Text = string.Empty;
                }
                else if (txtName.Text == PreviewText)
                {
                    txtName.Text = string.Empty;
                    btnCreate.IsEnabled = true;
                }
            };
        }

        public bool IsGlobal
        {
            get => this.chkGlobal.Visibility == Visibility.Visible && (this.chkGlobal.IsChecked ?? false);
            set => chkGlobal.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        public string Input
        {
            get { return txtName.Text?.Trim(); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
