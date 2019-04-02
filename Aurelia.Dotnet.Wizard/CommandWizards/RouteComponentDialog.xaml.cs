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
    public partial class RouteComponentDialog : Window
    {

        public RouteComponentDialog()
        {
            InitializeComponent();
            this.Loaded += RouteComponentDialog_Loaded;
            this.SetStyle();
        }

        private void RouteComponentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtRouteName.Focus();
            txtRouteName.CaretIndex = 0;
            txtRouteName.Text = PreviewText;
            txtRouteName.Select(0, txtRouteName.Text.Length);

            txtRouteName.PreviewKeyDown += (a, b) =>
            {
                if (b.Key == Key.Escape)
                {
                    if (string.IsNullOrWhiteSpace(txtRouteName.Text.Trim()) || txtRouteName.Text.Trim().Equals(PreviewText))
                        Close();
                    else
                        txtRouteName.Text = string.Empty;
                }

            };
        }

        public string ElementName
        {
            get { return txtRouteName.Text?.Trim(); }
        }

        public string Type
        {
            get
            {
                return rbBoth.IsChecked ?? false ? "both" : rbHtml.IsChecked ?? false ? "html" : "inline";
            }
        }

        public bool IsNav
        {
            get
            {
                return chkIsNav.IsChecked ?? false;
            }
        }
        public bool IsDefault
        {
            get
            {
                return chkIsNav.IsChecked ?? false;
            }
        }

        public string PreviewText => "Enter the name of the component you would like to create";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
