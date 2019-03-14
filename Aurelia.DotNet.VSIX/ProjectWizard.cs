#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Aurelia.DotNet.Wizard;
using EnvDTE;
using EnvDTE80;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.VisualStudio.TemplateWizard;

namespace Aurelia.DotNet.VSIX
{
    public class ProjectWizard : IWizard
    {
        private ProjectWizardViewModel _viewModel;
        private string solutionDirectory;
        private string projectDirectory;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var workingDirectory = this.solutionDirectory ?? this.projectDirectory;
            var parentDirectory = Directory.GetParent(workingDirectory);
            var directoryInfo = new DirectoryInfo(workingDirectory);
            var solution = Instance.DTE2.Solution.FullName;
            Instance.DTE2.Solution.Close(false);
            RemoveProjectDirectory();
            var process = new System.Diagnostics.Process
            {
                StartInfo = new ProcessStartInfo("dotnet")
                {
                    Arguments = $"new aurelia -o {directoryInfo.Name} --force --allow-scripts yes {_viewModel.ToString()}",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = parentDirectory.FullName
                }
            };
            process.Start();      
            process.WaitForExit();
            Instance.DTE2.Solution.Open(solution);
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (projectItem.Kind.ToLower() != "folder") { return; }
            if (projectItem.Name != "ClientApp") { return; }
        }

        public void RunFinished()
        {
        }

        public void RemoveSolutionDirectory()
        {
            if (!Directory.Exists(this.solutionDirectory))
            {
                return;
            }
            Directory.Delete(this.solutionDirectory, true);
        }

        public void RemoveProjectDirectory()
        {
            if (!Directory.Exists(this.projectDirectory))
            {
                return;
            }
            Directory.Delete(this.projectDirectory, true);
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                replacementsDictionary.TryGetValue("$solutiondirectory$", out this.solutionDirectory);
                replacementsDictionary.TryGetValue("$destinationdirectory$", out this.projectDirectory);
                var wiz = new DotNet.Wizard.ProjectWizard();
                wiz.ShowDialog();
                this._viewModel = wiz.ViewModel;
                if (_viewModel.Cancelled)
                {
                    throw new WizardBackoutException();
                }
                replacementsDictionary = replacementsDictionary.Union(_viewModel.ReplacementDictionary).ToDictionary(s => s.Key, s => s.Value);
            }
            catch (Exception ex)
            {
                RemoveSolutionDirectory();
                throw new WizardCancelledException("Wizard failed to create Aurelia Project Succesfully", ex);
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return false;
        }
    }
}
