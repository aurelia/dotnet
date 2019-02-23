using Aurelia.DotNet.Extensions.Models;
using System.Collections.ObjectModel;

namespace Aurelia.Dotnet.Wizard
{
    internal class ProjectWizardViewModel
    {
        public bool GenerateRoutes { get; set; }
        public StylesheetLanguage StyleSheet { get; set; }
        public ProjectWizardViewModel()
        {
            Routes = new ObservableCollection<Route>();
        }

        public ObservableCollection<Route> Routes { get; set; }
    }
}