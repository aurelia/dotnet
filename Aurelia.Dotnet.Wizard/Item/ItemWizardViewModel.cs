using Aurelia.DotNet.Extensions.Models;
using System.Collections.ObjectModel;

namespace Aurelia.Dotnet.Wizard
{
    internal class ItemWizardViewModel
    {
        public bool GenerateRoutes { get; set; }
        public StylesheetLanguage StyleSheet { get; set; }
        public ItemWizardViewModel()
        {
            Routes = new ObservableCollection<Route>();
        }

        public ObservableCollection<Route> Routes { get; set; }
    }
}