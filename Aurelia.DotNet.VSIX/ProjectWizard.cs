using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Aurelia.DotNet.Wizard;
using EnvDTE;
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
            Directory.GetDirectories(this.projectDirectory ?? this.solutionDirectory).ToList().ForEach(y => Directory.Delete(y, true));
            //Remove all non solution files as these will be regenerated using the dotnet aurelia templates
            Directory.GetFiles(this.projectDirectory).Where(y=> !Path.GetExtension(y).ToLower().Equals("sln")).ToList().ForEach(y =>  File.Delete(y));
            var process = new System.Diagnostics.Process
            {
                StartInfo = new ProcessStartInfo("dotnet")
                {
                    Arguments = "new aurelia --allow-scripts yes",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = this.projectDirectory ?? this.solutionDirectory
                }
            };
            process.Start();
            process.WaitForExit();
            //System.Threading.Thread.Sleep(1000); //We do this to make sure template folder won't be loaded on project startup.
            //var itemEnumerator = project.ProjectItems.GetEnumerator();
            //while (itemEnumerator.MoveNext())
            //{
            //    var item = itemEnumerator.Current as ProjectItem;
            //    if (item.Name.ToLower() != "clientapp") { continue; }
            //};
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
            return true;
        }
    }
}
