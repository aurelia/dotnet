using Aurelia.DotNet.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Aurelia.DotNet.Wizard
{
    /// <summary>
    /// Interaction logic for ItemWizard.xaml
    /// </summary>
    public partial class ItemWizard : Window
    {
        private ItemWizardViewModel viewModel { get => (ItemWizardViewModel)this.DataContext; }
        public ItemWizard()
        {
            InitializeComponent();
            this.DataContext = new ItemWizardViewModel();
            this.viewModel.Routes.Add(new Route
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
