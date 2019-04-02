using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using Aurelia.DotNet.Wizard.CommandWizards;
using Aurelia.DotNet.VSIX.Helpers;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using Aurelia.DotNet;
using System.Text.RegularExpressions;

namespace Aurelia.DotNet.VSIX.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateAureliaItem
    {
        private DTE2 _dte;

        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateAttribute"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateAureliaItem(AsyncPackage package, OleMenuCommandService commandService, DTE2 dte)
        {
            _dte = dte;
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(PackageGuids.guidAureliaCommandsSet, PackageIds.cmdGenerateAureliaItem);
            var menuItem = new OleMenuCommand(this.ExecuteAsync, menuCommandID);
            menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
            commandService.AddCommand(menuItem);
        }
        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;
            button.Visible = false;
            Helpers.DteHelpers.GetSelectionData(_dte, out var targetFolderPath, out _, out _);
            button.Visible = targetFolderPath.IsInAureliaSrcFolder();
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerateAureliaItem Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in Command1's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE2;

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new GenerateAureliaItem(package, commandService, dte);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void ExecuteAsync(object sender, EventArgs e)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var item = _dte.SelectedItems.Item(1);
                Helpers.DteHelpers.GetSelectionData(_dte, out var targetFile, out var projectFolderPath, out var projectFullName);
                var targetFolder = Path.GetDirectoryName(targetFile);

                if (string.IsNullOrEmpty(targetFolder) || !Directory.Exists(targetFolder))
                    return;

                var selectedItem = item as ProjectItem;
                var selectedProject = item as Project;
                var project = selectedItem?.ContainingProject ?? selectedProject ?? DteHelpers.GetActiveProject(_dte);

                var root = Directory.GetParent(AureliaHelper.RootFolder).FullName.Replace(@"\", @"\\"); //escape for pregex
                var relativePath = Regex.Replace(targetFolder, root, "", RegexOptions.IgnoreCase).Substring(1); // remove starting slash
                var fileNameDialog = new FileNameDialog(relativePath)
                {
                    Title = "Generate Aurelia Item",
                    IsGlobal = true,
                    PreviewText = "Please enter the name aurelia item to create using Aurelia Conventions"
                };
                var lowerFolder = targetFolder.ToLower();
                fileNameDialog.GlobalChecked(lowerFolder == AureliaHelper.GetResouceDirectory.ToLower() ||
                    lowerFolder == AureliaHelper.GetAttributesDirectory.ToLower() ||
                    lowerFolder == AureliaHelper.GetBindingBehaviorsDirectory.ToLower() ||
                    lowerFolder == AureliaHelper.GetElementsDirectory.ToLower() ||
                    lowerFolder == AureliaHelper.GetValueConvertersDirectory.ToLower() ||
                    targetFile.ToLower().Equals(AureliaHelper.ResourceGlobalFile.ToLower()

                    ));

                var dialog = _dte.OpenDialog(fileNameDialog);
                if (!(dialog.ShowDialog() ?? false)) { return; }
                var moduleName = dialog.Input;
                var aureliaType = AureliaHelper.ParseModuleName(moduleName);
                var templates = Template.GetTemplateFilesByType(aureliaType.ToString().ToLower()).ToList();
                var globalFolder = AureliaHelper.GetDirectory(aureliaType);
                targetFolder = dialog.IsGlobal && !string.IsNullOrWhiteSpace(globalFolder) ? globalFolder : targetFolder;

                var filesToOpen = await Task.WhenAll(templates.Select(templateName => Template.GenerateTemplatesAsync(templateName, targetFolder, dialog.Input, dialog.IsGlobal)));
                filesToOpen.ToList().ForEach(fullFileName => VsShellUtilities.OpenDocument(package, fullFileName));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }

        }
    }
}
