using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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


    }
}
