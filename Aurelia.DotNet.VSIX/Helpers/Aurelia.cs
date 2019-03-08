using Aurelia.DotNet.Extensions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurelia.DotNet.VSIX.Helpers
{
    public static class Aurelia
    {
        public static DateTime LastModifyDate { get; set; }
        public static AureliaCli AureliaCli { get; set; }
        public static string RootFolder { get; set; }
        public static bool? IsTypescript { get; set; }
        public const string AureliaFileName = "aurelia.json";
        public const string AureliaRouterSearchText = "configurerouter";
        public static string ResourceGlobalFile { get; set; }
        public static bool IsWebpack { get; set; }

        public static bool IsAureliaCliFile(this string fileName) => fileName.ToLower().Equals(AureliaFileName);


        public static bool IsAureliaRouter(this string fileName) => File.ReadAllText(fileName).ToLower().Contains(AureliaRouterSearchText);

        public static void LoadAureliaCli(string aureliaFile)
        {
            var jsonFile = File.ReadAllText(aureliaFile);
            AureliaCli = JsonConvert.DeserializeObject<AureliaCli>(jsonFile);

            var currentDirectory = Directory.GetParent(aureliaFile);
            var currentPath = Path.Combine(currentDirectory.FullName, AureliaCli.Paths.Root);
            RootFolder = Directory.Exists(currentPath) ? currentPath : Path.Combine(currentDirectory.Parent.FullName, AureliaCli.Paths.Root).ToLower();
            IsTypescript = AureliaCli.Transpiler.Id.ToLower().Equals("typescript");
            IsWebpack = string.IsNullOrWhiteSpace(AureliaCli.Bundler?.Id ?? string.Empty) || AureliaCli.Bundler.Id == "webpack";
            var resourceDir = Path.Combine(RootFolder, AureliaCli.Paths.Resources);
            ResourceGlobalFile = Directory.EnumerateFiles(resourceDir, "index.*", SearchOption.TopDirectoryOnly).FirstOrDefault();
        }

        public static bool IsInAureliaRoot(string targetFileOrFolder, string projectDirectory)
        {
            var aureliaProjectDirectory = Directory.EnumerateDirectories(projectDirectory, "aurelia_project", SearchOption.AllDirectories).FirstOrDefault();
            if (aureliaProjectDirectory == null) { return false; }
            var aureliaFile = Path.Combine(aureliaProjectDirectory, AureliaFileName);
            if (!File.Exists(aureliaFile)) { return false; }
            var lastWriteTime = File.GetLastWriteTimeUtc(aureliaFile);
            if (lastWriteTime > LastModifyDate)
            {
                var jsonFile = File.ReadAllText(aureliaFile);
                AureliaCli = JsonConvert.DeserializeObject<AureliaCli>(jsonFile);
                LastModifyDate = lastWriteTime;
                RootFolder = Path.Combine(Directory.GetParent(aureliaProjectDirectory).FullName, AureliaCli.Paths.Root).ToLower();
            }

            if (AureliaCli.Paths == null) { return false; }
            if (IsTypescript == null)
            {
                IsTypescript = AureliaCli.Transpiler.Id.ToLower().Equals("typescript");
            }
            IsWebpack = string.IsNullOrWhiteSpace(AureliaCli.Bundler?.Id ?? string.Empty) || AureliaCli.Bundler.Id == "webpack";
            return targetFileOrFolder.ToLower().Contains(RootFolder);

        }

        public static bool IsInAureliaSrcFolder(this string targetFileOrFolder) => targetFileOrFolder.ToLower().Contains(RootFolder);

        public static void LoadAureliaCliFromPath(string path)
        {
            var aureliaProjectDirectory = Directory.EnumerateDirectories(Directory.GetParent(path).FullName, "aurelia_project", SearchOption.AllDirectories).FirstOrDefault();
            if (aureliaProjectDirectory == null) { return; }
            var aureliaFile = Path.Combine(aureliaProjectDirectory, AureliaFileName);
            if (!File.Exists(aureliaFile)) { return; }
            LoadAureliaCli(aureliaFile);
        }

        public static string GetResouceDirectory => Path.Combine(RootFolder, AureliaCli.Paths.Resources);
        public static string GetElementsDirectory => Path.Combine(RootFolder, AureliaCli.Paths.Elements);
        public static string GetAttributesDirectory => Path.Combine(RootFolder, AureliaCli.Paths.Attributes);
        public static string GetBindingBehaviorsDirectory => Path.Combine(RootFolder, AureliaCli.Paths.BindingBehaviors);
        public static string GetValueConvertersDirectory => Path.Combine(RootFolder, AureliaCli.Paths.ValueConverters);

        public static void AddGlobalResource(string fullFileName)
        {
            var resourceDir = Path.Combine(RootFolder, AureliaCli.Paths.Resources);
            var resourceGlobal = Directory.EnumerateFiles(resourceDir, "index.*", SearchOption.TopDirectoryOnly).FirstOrDefault();
            var resourceFile = File.ReadAllText(ResourceGlobalFile);
            var configurationSettings = Regex.Match(resourceFile, @"(\/{0}|\/{2})(config.globalResources\(\[((.|\n)*)\]\))");
            if (configurationSettings.Groups.Count > 3)
            {

                var currentText = configurationSettings.Groups[0].Value;
                var currentValue = configurationSettings.Groups[2].Value;
                var currentPaths = configurationSettings.Groups[3].Value;
                var replaceMentText = currentPaths;
                Uri folder = new Uri(ResourceGlobalFile);
                Uri newFile = new Uri(fullFileName);
                var relativePath = $"'{folder.MakeRelativeUri(newFile).ToString().Replace('/', Path.DirectorySeparatorChar)}'";

                if (IsWebpack)
                {
                    relativePath = $"PLATFORM.moduleName({relativePath})";
                }


                replaceMentText += string.IsNullOrWhiteSpace(currentPaths) || currentPaths.EndsWith(",") ? relativePath : ", " + relativePath;

                replaceMentText = replaceMentText.StartsWith("[") ? replaceMentText : $"[{replaceMentText}]";

                if (!resourceFile.Contains("PLATFORM"))
                {
                    resourceFile = @"import { PLATFORM } from 'aurelia-framework';" + Environment.NewLine + resourceFile;
                }
                resourceFile = resourceFile.Replace(currentText, currentValue.Replace(string.IsNullOrWhiteSpace(currentPaths) ? "[]" : $"[{currentPaths}]", replaceMentText));
                File.WriteAllText(ResourceGlobalFile, resourceFile);
            }

        }
    }
}
