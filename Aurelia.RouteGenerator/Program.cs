using Aurelia.DotNet.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Aurelia.RouteGenerator
{
    class Program
    {

        private const string TypescriptImport = "import {RouterConfiguration, Router} from 'aurelia-router';";
        private const string WebPackImport = "  import { PLATFORM } from 'aurelia-framework';";
        private const string TypescriptConfigure = "    configureRouter(config: RouterConfiguration, router: Router): void {";
        private const string JSConfigure = "    configureRouter(config, router): void {";


        private const string RouterDefaultLayoutTS =
@"{3}
  
  export class {0} {
    router: Router;
  
    configureRouter(config: RouterConfiguration, router: Router): void {
      this.router = router;
      config.title = '{1}';
      config.map({2});
    }
  }";


        private const string RouterDefaultLayoutJs =
@"import {RouterConfiguration, Router} from 'aurelia-router';
  
  export class {0} {
    router: Router;
  
    configureRouter(config: RouterConfiguration, router: Router): void {
      this.router = router;
      config.title = '{1}';
      config.map({2});
    }
  }";

        static void Main(string[] args)
        {
            var routes = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Route>>("");

        }
    }
}
