using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aurelia.DotNet.Extensions;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Aurelia.DotNet.VSIX.Helpers
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
        public static IEnumerable<string> GetTemplateFilesByType(string type) => TemplateFiles.Where(y => (y.Contains((Aurelia.IsTypescript ?? false ? "ts" : "js")) || y.Contains("html")) && y.Contains(type)).ToList();


        public static async Task GenerateTemplatesAsync(Package package, string templateName, string targetFolder, string elementName, bool isGlobal = false)
        {
            using (var reader = new StreamReader(templateName))
            {
                var fileName = Path.GetFileName(templateName);
                var parts = fileName.Split('.');
                var extension = parts[parts.Length - 2];
                var pascalCaseElementName = elementName.ToPascalCase();
                var kebabCase = pascalCaseElementName.PascalToKebabCase();
                var fullFileName = Path.Combine(targetFolder, kebabCase, $"{kebabCase}.{extension}");
                string templateText = await reader.ReadToEndAsync();
                templateText = templateText.Replace("%name%", pascalCaseElementName);
                templateText = templateText.Replace("%properties%", "");
                Directory.CreateDirectory(Path.GetDirectoryName(fullFileName));
                File.WriteAllText(fullFileName, templateText);

                if (isGlobal && (extension.ToLower() == "js" || extension.ToLower() == "ts"))
                {
                    Helpers.Aurelia.AddGlobalResource(fullFileName);
                }

                VsShellUtilities.OpenDocument(package, fullFileName);

            }
        }

    }
}
