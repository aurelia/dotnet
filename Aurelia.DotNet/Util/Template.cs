using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Aurelia.DotNet
{
    public static class Template
    {
        public static readonly string _folder;
        public static IEnumerable<string> TemplateFiles { get; }
        public const string _defaultExt = ".txt";

        static Template()
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            _folder = Path.Combine(Path.GetDirectoryName(assembly), "Templates");
            TemplateFiles = Directory.GetFiles(_folder, "*.txt", SearchOption.AllDirectories).Select(y => y.ToLower()).ToList();
        }


        public static string GetTemplateText(string templateName) => TemplateFiles.FirstOrDefault(y => Path.GetFileNameWithoutExtension(y.ToLower()).Equals(templateName.ToLower()));
        public static IEnumerable<string> GetTemplateFilesByType(string type) =>
            TemplateFiles.Where(templatePath => (templatePath.Contains((AureliaHelper.IsTypescript ?? false ? "ts" : "js")) || templatePath.Contains("html")) &&
            Path.GetFileName(templatePath).Split('.').Any(fileParts => fileParts?.ToLower().Equals(type) ?? false)).ToList();


        public static async Task<string> GenerateTemplatesAsync(string templateName, string targetFolder, string elementName, bool isGlobal = false)
        {
            string templateText;
            using (var reader = new StreamReader(templateName))
            {
                templateText = await reader.ReadToEndAsync();
            }

            var customIndex = elementName.IndexOf("Custom", StringComparison.InvariantCultureIgnoreCase);
            customIndex = customIndex > 0 ? customIndex : elementName.IndexOf("BindingBehavior", StringComparison.InvariantCultureIgnoreCase);
            customIndex = customIndex > 0 ? customIndex : elementName.IndexOf("ValueConverter", StringComparison.InvariantCultureIgnoreCase);
            elementName = elementName.Substring(0, customIndex > 0 ? customIndex : elementName.Length);

            var fileName = Path.GetFileName(templateName);
            var parts = fileName.Split('.');
            var extension = parts[parts.Length - 2];
            var pascalCaseElementName = elementName.ToPascalCase();
            var kebabCase = pascalCaseElementName.PascalToKebabCase();
            var fullFileName = Path.Combine(targetFolder, kebabCase, $"{kebabCase}.{extension}");
            templateText = templateText.Replace("%name%", pascalCaseElementName);
            templateText = templateText.Replace("%properties%", "");
            Directory.CreateDirectory(Path.GetDirectoryName(fullFileName));
            File.WriteAllText(fullFileName, templateText);

            if (templateName.Equals("feature"))
            {
                AureliaHelper.AddFeatureToConfigure(targetFolder);
            }

            if (isGlobal && (extension.ToLower() == "js" || extension.ToLower() == "ts"))
            {
                AureliaHelper.AddGlobalResource(fullFileName);
            }
            return fullFileName;
        }

    }
}
