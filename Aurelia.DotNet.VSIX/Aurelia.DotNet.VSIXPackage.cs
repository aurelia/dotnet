#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using Aurelia.DotNet.VSIX.Helpers;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;

namespace Aurelia.DotNet.VSIX
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(AVSIXPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasSingleProject, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class AVSIXPackage : AsyncPackage
    {
        /// <summary>
        /// Aurelia.Tools.DotNet.VSIXPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "dae5925d-6027-4259-b6c3-2d268e932b09";

        public AVSIXPackage()
        {
            Instance.Package = this;
            Instance.DTE2 = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as EnvDTE80.DTE2;
            Instance.Shell = ServiceProvider.GlobalProvider.GetService(typeof(SVsShell)) as IVsShell;
            Instance.Solution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution2;
            Instance.Solution4 = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution4;
            Instance.Watcher = new Watcher();
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            Logger.Initialize(this, VsixInfo.Name);

            await Aurelia.DotNet.VSIX.Commands.GenerateElement.InitializeAsync(this);
            await Aurelia.DotNet.VSIX.Commands.GenerateRoute.InitializeAsync(this);
            await Aurelia.DotNet.VSIX.Commands.GenerateAttribute.InitializeAsync(this);
            await Aurelia.DotNet.VSIX.Commands.GenerateAureliaItem.InitializeAsync(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Instance.Watcher.Dispose();
        }


        #endregion
    }
}
