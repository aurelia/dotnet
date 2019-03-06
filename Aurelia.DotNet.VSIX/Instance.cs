using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;

namespace Aurelia.DotNet.VSIX
{
    class Instance
    {
        public static AVSIXPackage Package { get; internal set; }
        public static IVsSolution2 Solution { get; internal set; }
        public static DTE2 DTE2 { get; internal set; }
        public static IVsShell Shell { get; internal set; }
        public static Watcher Watcher { get; internal set; }
    }
}
