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
    public partial class ElementGenerationDialog : Window
    {

        public ElementGenerationDialog()
        {
            InitializeComponent();
            this.Loaded += ElementGenerationDialog_Loaded;
            this.SetStyle();
        }

        private void ElementGenerationDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtElementName.Focus();
            txtElementName.CaretIndex = 0;
            txtPropertyNames.Text = PropertyNamesPreviewText;
            txtElementName.Select(0, txtElementName.Text.Length);

            txtElementName.PreviewKeyDown += (a, b) =>
            {
                if (b.Key == Key.Escape)
                {
                    if (string.IsNullOrWhiteSpace(txtElementName.Text))
                        Close();
                    else
                        txtElementName.Text = string.Empty;
                }

            };
        }

        public string ElementName
        {
            get { return txtElementName.Text?.Trim(); }
        }

        public string Type
        {
            get
            {
                return rbBoth.IsChecked ?? false ? "both" : rbHtml.IsChecked ?? false ? "html" : "inline";
            }
        }

        public string PropertyNames
        {
            get
            {
                return txtPropertyNames.Text?.ToLower().Equals(PropertyNamesPreviewText.ToLower()) ?? false ? string.Empty : txtPropertyNames.Text?.Trim();
            }
        }

        public void GlobalChecked(bool value)
        {
            this.chkGlobal.IsChecked = value;
        }

        public bool IsGlobal
        {
            get
            {
                return chkGlobal.IsChecked ?? false;
            }
        }

        public string PropertyNamesPreviewText => "Enter the bindable property names delimted by a , (eg. FirstNaame, LastName)";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
