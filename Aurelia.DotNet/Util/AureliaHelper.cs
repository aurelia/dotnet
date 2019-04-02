using Aurelia.DotNet.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aurelia.DotNet
{
    public static class AureliaHelper
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
                RootFolder = Path.Combine(Directory.GetParent(aureliaProjectDirectory).FullName, AureliaCli.Paths.Root ?? "").ToLower();
            }

            if (AureliaCli.Paths == null) { return false; }
            if (IsTypescript == null)
            {
                IsTypescript = AureliaCli.Transpiler.Id.ToLower().Equals("typescript");
            }
            IsWebpack = string.IsNullOrWhiteSpace(AureliaCli.Bundler?.Id ?? string.Empty) || AureliaCli.Bundler.Id == "webpack";
            return targetFileOrFolder.ToLower().Contains(RootFolder);

        }

        internal static void AddFeatureToConfigure(string targetFolder)
        {

        }

        public static bool IsInAureliaSrcFolder(this string targetFileOrFolder) => targetFileOrFolder.ToLower().Contains(RootFolder ?? "");

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

        public static void AddRoute(string targetFile, string fullFileName)
        {
            var routerFile = File.ReadAllText(targetFile);
            var configurationSettings = Regex.Match(routerFile, @"(\/{0}|\/{2})(config.map\(\[((.|\n)*)\]\))");
            if (configurationSettings.Groups.Count > 3)
            {
                var currentText = configurationSettings.Groups[0].Value;
                var currentValue = configurationSettings.Groups[2].Value;
                var currentPaths = configurationSettings.Groups[3].Value;
                var replaceMentText = currentPaths;
                string relativePath = GetRelativePath(targetFile, fullFileName);

                if (IsWebpack)
                {
                    relativePath = $"PLATFORM.moduleName({relativePath})";
                }

                replaceMentText += string.IsNullOrWhiteSpace(currentPaths) || currentPaths.EndsWith(",") ? relativePath : ", " + relativePath;

                replaceMentText = replaceMentText.StartsWith("[") ? replaceMentText : $"[{replaceMentText}]";

                if (!routerFile.Contains("PLATFORM"))
                {
                    routerFile = @"import { PLATFORM } from 'aurelia-framework';" + Environment.NewLine + routerFile;
                }
                routerFile = routerFile.Replace(currentText, currentValue.Replace(string.IsNullOrWhiteSpace(currentPaths) ? "[]" : $"[{currentPaths}]", replaceMentText));
                File.WriteAllText(targetFile, routerFile);
            }

        }

        public static AureliaItemType ParseModuleName(string moduleName)
        {
            var moduleNameTrimmed = moduleName?.Trim();
            if (string.IsNullOrWhiteSpace(moduleNameTrimmed)) { return AureliaItemType.Component; }

            if (moduleNameTrimmed.Contains("CustomElement"))
            {
                return AureliaItemType.Element;
            }

            if (moduleNameTrimmed.Contains("CustomAttribute"))
            {
                return AureliaItemType.Attribute;
            }
            if (moduleNameTrimmed.Contains("ValueConverter"))
            {
                return AureliaItemType.ValueConverter;
            }
            if (moduleNameTrimmed.Contains("BindingBehavior"))
            {
                return AureliaItemType.BindingBehavior;
            }
            if (moduleNameTrimmed.Contains("Feature"))
            {
                return AureliaItemType.Feature;
            }
            if (moduleNameTrimmed.Contains("Route"))
            {
                return AureliaItemType.Route;
            }
            return AureliaItemType.Component;
        }

        public static string GetDirectory(AureliaItemType aureliaType)
        {
            switch (aureliaType)
            {
                case AureliaItemType.Attribute:
                    return GetAttributesDirectory;
                case AureliaItemType.Element:
                    return GetElementsDirectory;
                case AureliaItemType.ValueConverter:
                    return GetValueConvertersDirectory;
                case AureliaItemType.BindingBehavior:
                    return GetBindingBehaviorsDirectory;
                case AureliaItemType.Route:
                case AureliaItemType.Component:
                case AureliaItemType.Feature:
                default:
                    return null;
            }
        }

        private static string GetRelativePath(string targetFile, string fullFileName)
        {
            Uri folder = new Uri(targetFile);
            Uri newFile = new Uri(fullFileName);
            var relativeFileName = folder.MakeRelativeUri(newFile).ToString();
            var module = relativeFileName.Replace(Path.GetExtension(relativeFileName), "");
            return $"'./{module}'";
        }

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
                string relativePath = GetRelativePath(ResourceGlobalFile, fullFileName);

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
