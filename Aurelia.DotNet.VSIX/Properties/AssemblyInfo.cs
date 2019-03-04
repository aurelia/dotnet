using Aurelia.DotNet.VSIX;
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(VsixInfo.Name)]
[assembly: AssemblyDescription(VsixInfo.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(VsixInfo.Author)]
[assembly: AssemblyProduct(VsixInfo.Name)]
[assembly: AssemblyCopyright(VsixInfo.Author)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture(VsixInfo.Language)]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: NeutralResourcesLanguage(VsixInfo.Language)]

[assembly: AssemblyVersion(VsixInfo.Version)]
[assembly: AssemblyFileVersion(VsixInfo.Version)]