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
using Aurelia.DotNet.Extensions;

namespace Aurelia.DotNet.VSIX.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateAttribute
    {
        private DTE2 _dte;

        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateAttribute"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateAttribute(AsyncPackage package, OleMenuCommandService commandService, DTE2 dte)
        {
            _dte = dte;
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(PackageGuids.guidAureliaCommandsSet, PackageIds.cmdGenerateAttribute);
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
        public static GenerateAttribute Instance
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
            Instance = new GenerateAttribute(package, commandService, dte);
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
            var item = _dte.SelectedItems.Item(1);
            Helpers.DteHelpers.GetSelectionData(_dte, out var targetFile, out var projectFolderPath, out var projectFullName);
            var targetFolder = Path.GetDirectoryName(targetFile);

            if (string.IsNullOrEmpty(targetFolder) || !Directory.Exists(targetFolder))
                return;

            var selectedItem = item as ProjectItem;
            var selectedProject = item as Project;
            var project = selectedItem?.ContainingProject ?? selectedProject ?? DteHelpers.GetActiveProject(_dte);


            var dialog = DteHelpers.OpenDialog<FileNameDialog>(_dte);
            dialog.Title = "Generate Attribute";
            var relativePath = targetFolder.Substring(targetFolder.IndexOf(Aurelia.DotNet.VSIX.Helpers.Aurelia.RootFolder));
            dialog.FolderLabel = relativePath;
            dialog.PreviewText = "Please enter the name of the attribute you would like to create. eg. RedBox";
            if (!(dialog.ShowDialog() ?? false)) { return; }
            var elementName = dialog.Name;
            var templates = Template.GetTemplateFilesByType("attribute").ToList();
            targetFolder = dialog.IsGlobal ? Helpers.Aurelia.GetElementsDirectory : targetFolder;

            await Task.WhenAll(templates.Select(templateName => Template.GenerateTemplatesAsync(package, templateName, targetFolder, dialog.Input, dialog.IsGlobal)));


        }
    }
}
