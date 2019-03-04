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

        public static bool? IsTypescript { get; set; }

        public static Task<bool> IsInAureliaRoot(string targetFileOrFolder, string projectDirectory)
        {
            return Task.Run(() =>
            {
                var aureliaProjectDirectory = Directory.GetDirectories(projectDirectory, "aurelia_project", SearchOption.AllDirectories).FirstOrDefault();
                if (aureliaProjectDirectory == null) { return false; }
                var aureliaFile = Path.Combine(aureliaProjectDirectory, "aurelia.json");
                if (!File.Exists(aureliaFile)) { return false; }
                var jsonFile = File.ReadAllText(aureliaFile);
                var configFile = JsonConvert.DeserializeObject<AureliaCli>(jsonFile);
                if (configFile.Paths == null) { return false; }
                if (IsTypescript == null)
                {
                    IsTypescript = configFile.Transpiler.Id.ToLower().Equals("typescript");
                }

                var folder = Path.Combine(Directory.GetParent(aureliaProjectDirectory).FullName, configFile.Paths.Root).ToLower();
                return targetFileOrFolder.ToLower().Contains(folder);
            });
        }
    }
}
