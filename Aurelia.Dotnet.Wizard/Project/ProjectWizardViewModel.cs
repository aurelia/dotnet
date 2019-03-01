using Aurelia.DotNet.Extensions;
using Aurelia.DotNet.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Aurelia.Dotnet.Wizard
{
    public class ProjectWizardViewModel
    {
        public ProjectWizardViewModel()
        {
            this.StylesheetLanguage = StylesheetLanguage.SCSS;
            this.Transpiler = Transpiler.TypeScript;
            this.HttpProtocol = HttpProtocol.HTTP1;
            this.LoaderBundle = LoaderBundle.Webpack;
            this.PackageManager = PackageManager.Yarn;
            Routes = new ObservableCollection<Route>();
            Port = 8080;
        }


        public bool GenerateRoutes { get; set; }
        [CliProperty("cssProcessor")]
        public StylesheetLanguage StylesheetLanguage { get; set; }
        [CliProperty("")]
        public PackageManager PackageManager { get; set; }
        [CliProperty("transpiler")]
        public Transpiler Transpiler { get; set; }
        [CliProperty("http")]
        public HttpProtocol HttpProtocol { get; set; }
        [CliProperty("bundler")]
        public LoaderBundle LoaderBundle { get; set; }
        [CliProperty("postProcessor")]
        public PostProcessor PostProcessor { get; set; }
        [CliProperty("unitTesting")]
        public UnitTesting UnitTesting { get; set; }
        [CliProperty("integrationTestingg")]
        public IntegrationTesting IntegrationTesting { get; set; }
        [CliProperty("minification")]
        public Minification Minification { get; set; }
        [CliProperty("editor")]
        public Editor Editor { get; set; }


        [CliProperty("port")]
        public int Port { get; set; }
        public string Folder { get; set; }

        public ObservableCollection<Route> Routes { get; set; }
        public bool Cancelled { get; internal set; }
        public Dictionary<string, string> ReplacementDictionary
        {
            get
            {

                var result = new Dictionary<string, string>();
                var properties = this.GetType().GetProperties().Where(y=>y.Name != nameof(ReplacementDictionary));
                properties.ToList().ForEach(prop =>
                {
                    result.Add($"${prop.Name}$", prop.GetValue(this)?.ToString());
                });
                return result;
            }
        }
    }
}