using Aurelia.DotNet.Models;
using System.Collections.ObjectModel;

namespace Aurelia.DotNet.Wizard
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