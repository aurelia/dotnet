using Aurelia.DotNet.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Aurelia.DotNet.Wizard
{
    public class ProjectWizardViewModel : INotifyPropertyChanged
    {
        public ProjectWizardViewModel()
        {
            this.StylesheetLanguage = StylesheetLanguage.None;
            this.Transpiler = Transpiler.TypeScript;
            this.HttpProtocol = HttpProtocol.HTTP1;
            this.LoaderBundle = LoaderBundle.Webpack;
            this.PackageManager = PackageManager.Yarn;
            Routes = new ObservableCollection<Route>()
            {
                new Route
                {
                    Title = "App",
                }

            };
            Port = 8080;
            GenerateRoutes = true;
            this.CurrentRoute = new Route();
        }


        public bool GenerateRoutes { get; set; }
        [CliProperty("cssProcessor")]
        public StylesheetLanguage StylesheetLanguage { get; set; }
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
        [CliProperty("integrationTesting")]
        public IntegrationTesting IntegrationTesting { get; set; }
        [CliProperty("minification")]
        public Minification Minification { get; set; }
        [CliProperty("editor")]
        public Editor Editor { get; set; }

        [CliProperty("secure")]
        public bool Secure { get; set; }

        [CliProperty("signalR")]
        public bool SignalR { get; set; }


        public int Port { get; set; }
        public string Folder { get; set; }
        private Route _currentRoute;
        public Route CurrentRoute
        {
            get { return _currentRoute; }
            set
            {
                _currentRoute = value;
                OnPropertyChanged(nameof(CurrentRoute));
            }
        }

        public ObservableCollection<Route> Routes { get; set; }
        public bool Cancelled { get; internal set; }
        public Dictionary<string, string> ReplacementDictionary
        {
            get
            {

                var result = new Dictionary<string, string>();
                var properties = this.GetType().GetProperties().Where(y => y.Name != nameof(ReplacementDictionary));
                properties.ToList().ForEach(prop =>
                {
                    result.Add($"${prop.Name}$", prop.GetValue(this)?.ToString());
                });
                return result;
            }
        }

        protected virtual void OnPropertyChanged(string property)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public override string ToString()
        {
            return string.Join("", this.GetType().GetProperties().Where(y=>y.GetCustomAttributes(typeof(CliPropertyAttribute), false).Any()).ToList().Select(y => $"--{((CliPropertyAttribute)y.GetCustomAttributes(typeof(CliPropertyAttribute), false).FirstOrDefault()).Name} {y.GetValue(this).ToString().ToLower()} "));
        }
    }
}