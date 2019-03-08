using Aurelia.DotNet.Wizard.CommandWizards;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Aurelia.DotNet.Wizard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {

            var viewModel = Parse(e.Args);
            ProjectWizard mainWindow = new ProjectWizard
            {
                ViewModel = viewModel
            };
            //var mainWindow = new ItemWizard();
            mainWindow.Show();
        }

        /// <summary>
        /// Could get more complicated who knows don't want to bring in third party libs since this will be embedded in VS as well
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ProjectWizardViewModel Parse(string[] args)
        {
            var folderName = args.Any() ? args[0] : Environment.CurrentDirectory;
            return new ProjectWizardViewModel { Folder = folderName };
        }
    }
}
