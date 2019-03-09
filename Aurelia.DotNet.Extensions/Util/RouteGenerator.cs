using System;
using System.Collections.Generic;
using System.Text;
using Aurelia.DotNet.Models;

namespace Aurelia.DotNet.Util
{
    public class RouteGenerator
    {
        private readonly Route[] _routes;
        private RouteGenerator(params Models.Route[] routes)
        {
            _routes = routes;
        }

        public IEnumerable<Route> Routes => _routes;



        public RouteGenerator AddRoute(Route route, Route parentRoute = null)
        {
            return this;
        }

        public RouteGenerator RemoveRoute(Route route)
        {
            return this;
        }

        public static RouteGenerator Generate(string routeDirectory, params Models.Route[] routes)
        {
            var generator = new RouteGenerator(routes);
            generator.Generate();
            return generator;
        }

        public void Generate()
        {

        }

    }
}
