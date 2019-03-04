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

        public static bool IsInAureliaRoot(string targetFileOrFolder, string projectDirectory)
        {
            var aureliaProjectDirectory = Directory.GetDirectories(projectDirectory, "aurelia_project", SearchOption.AllDirectories).FirstOrDefault();
            if (aureliaProjectDirectory == null) { return false; }
            var aureliaFile = Path.Combine(aureliaProjectDirectory, "aurelia.json");
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

            return targetFileOrFolder.ToLower().Contains(RootFolder);

        }
    }
}
