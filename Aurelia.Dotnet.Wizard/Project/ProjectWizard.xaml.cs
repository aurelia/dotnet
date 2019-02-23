using Aurelia.DotNet.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Aurelia.Dotnet.Wizard
{
    /// <summary>
    /// Interaction logic for ProjectWizard.xaml
    /// </summary>
    public partial class ProjectWizard : Window
    {
        private ProjectWizardViewModel viewModel { get => (ProjectWizardViewModel)this.DataContext; }
        public ProjectWizard()
        {
            InitializeComponent();
            this.DataContext = new ProjectWizardViewModel();
            this.viewModel.Routes.Add(new DotNet.Extensions.Models.Route
            {
                ModuleName = "asdfasdf",
                Name = "adfsafsd",
                Title = "asdfasdf",
                CanNavigate = true,
                ChildRoutes = new ObservableCollection<Route>
                {
                    new Route
                    {
                        ModuleName = "asdfasdf",
                        Name = "adfsafsd",
                        Title = "asdfasdf",
                        CanNavigate = true,
                    }
                }
            });
        }

        private void SelectAllOnFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {

        }
    }
}
