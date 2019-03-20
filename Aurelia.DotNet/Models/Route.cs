using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Aurelia.DotNet.Models
{
    public class Route
    {
        public Route()
        {
            this.ChildRoutes = new ObservableCollection<Route>();
            this.CanNavigate = true;
        }

        public bool DefaultRoute { get; set; }
        public string Title { get; set; }
        public bool CanNavigate { get; set; }
        public bool DetailRoute { get; set; }
        public string Name { get; set; }
        public List<string> Paths { get; set; }
        public string ModuleName { get; set; }
        public ICollection<Route> ChildRoutes { get; set; }
        public string Href { get; set; }
    }
}

