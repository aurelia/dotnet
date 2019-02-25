using Aurelia.DotNet.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public event EventHandler<ProjectWizardViewModel> ChangesSaved;

        public ProjectWizardViewModel ViewModel { get => (ProjectWizardViewModel)this.DataContext; set => this.DataContext = value; }
        public ProjectWizard()
        {
            InitializeComponent();
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Aurelia.Dotnet.Wizard;component/aurelia-logo.png", UriKind.RelativeOrAbsolute);
            logo.EndInit();
            this.imgAurelia.Source = logo;
            this.MouseDown += ProjectWizard_MouseDown;
            this.DataContext = new ProjectWizardViewModel();
            this.ViewModel.Routes.Add(new DotNet.Extensions.Models.Route
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

        private void ProjectWizard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) { return; }
            this.DragMove();
        }


        private void SelectAllOnFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            // Emit event to let people gather settings
            this.ChangesSaved?.Invoke(this, this.ViewModel);
            this.Close();
        }

        private void CancelChanges(object sender, RoutedEventArgs e)
        {
            ViewModel.Cancelled = true;
            this.Close();
        }

        private void TxtPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
