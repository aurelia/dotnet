using Aurelia.DotNet.Extensions.Models;
using Aurelia.DotNet.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aurelia.RouteGenerator
{
    class Program
    {

        private const string TypescriptImport = "import {RouterConfiguration, Router} from 'aurelia-router';";
        private const string WebPackImport = "import { PLATFORM } from 'aurelia-framework';";
        private const string TypescriptConfigure = "configureRouter(config: RouterConfiguration, router: Router): void";
        private const string JSConfigure = "configureRouter(config, router)";
        private const string TypescriptRouter = "private router: Router;";
        private const string JSRouter = "router;";
        private const string RouterDefaultLayout =
@"export class {0} {{
    {4}
    {5}
    {1} {{
        this.router = router;
        config.title = '{2}';
        config.map(
            {3}
        );
    }}
}}";

        private static JsonSerializer serializer = new JsonSerializer();

        private const string Extension =
#if TYPESCRIPT
                ".ts";
#else
                ".js";
#endif



        static void Main(string[] args)
        {

            var jsonString = args.Any() ? args[0] : "%routes%";


            var routes = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Route>>(jsonString);
            GenerateRouteFile(routes);

        }

        private static void GenerateRouteFile(IEnumerable<Route> routes, string file = null)
        {
            if (!routes.Any()) { return; }
            var serializedRoutes = string.Empty;
            using (var stringWriter = new StringWriter())
            using (var writer = new JsonTextWriter(stringWriter))
            {
                writer.QuoteName = false;
                writer.QuoteChar = '\'';
                writer.Formatting = Formatting.None;

                serializer.Serialize(writer, routes.Select(route => new
                {
                    route = route.DefaultRoute ? route.Paths.Append("") : route.Paths,
                    name = route.Name,
                    moduleId = "./" + route.ModuleName.ToPascalCase().PascalToKebabCase() + "/index",
                    nav = route.CanNavigate,
                    title = route.Title ?? route.Name,
                    href = route.Href ?? $"#{route.Name}"
                }));
                serializedRoutes = stringWriter.ToString().Replace("},", "}," + Environment.NewLine + "\t\t\t ");
            }

#if WEBPACK
            serializedRoutes = Regex.Replace(serializedRoutes, "(moduleId:)(.*?),",
                x => x.Groups[1].Value + "PLATFORM.moduleName(" + x.Groups[2].Value + "),"
                );
#endif


            if (!routes.Any()) { return; }
            file = file?.Replace(Extension, "") ?? "app";
            var sb = new StringBuilder();
            sb.AppendLine("// SEE - https://aurelia.io/docs/routing/configuration#basic-configuration for more routing information");
#if WEBPACK
            sb.AppendLine(WebPackImport);
#endif
#if TYPESCRIPT
            sb.AppendLine(TypescriptImport);
#endif

            sb.AppendFormat(RouterDefaultLayout, GetModuleName(file),
#if TYPESCRIPT
                    TypescriptConfigure,
#else
                    JSConfigure,
#endif
                    file.Equals("app") ? "Aurelia" : GetModuleName(file), serializedRoutes,

#if TYPESCRIPT
                    TypescriptRouter
#else
                    JSRouter
#endif
                    , $"message = 'Hello from {GetModuleName(file)}';"


                    );



            File.WriteAllText(file + Extension, sb.ToString());
            File.WriteAllText(file + ".html", $"<template>${{message}} <br/> <router-view /></template>");

            routes.ToList().ForEach(y =>
            {
                GenerateFile(file.Equals("app") ? "" : Path.GetDirectoryName(file), y);
            });
        }

        private static string GetModuleName(string file)
        {
            return file.ToPascalCase().Replace("\\index", "");
        }

        private static void GenerateFile(string parentRouteFile, Route route)
        {
            var directoryToCreate = Path.Combine(parentRouteFile, route.ModuleName.ToLower());
            Directory.CreateDirectory(directoryToCreate);
            var tsJsFile = Path.Combine(directoryToCreate, "index" + Extension);
            var htmlFile = Path.Combine(directoryToCreate, "index.html");
            File.WriteAllText(tsJsFile, $"export class {route.ModuleName.ToPascalCase()} {{message = 'Hello from {route.ModuleName.ToPascalCase()}'; }}");
            File.WriteAllText(htmlFile, $"<template>${{message}} <br/> {(route.ChildRoutes.Any() ? "<router-view />" : "")}   </template>");
            GenerateRouteFile(route.ChildRoutes, tsJsFile);


        }
    }
}
;