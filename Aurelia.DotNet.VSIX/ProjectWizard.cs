using System.Collections.Generic;
using Aurelia.Dotnet.Wizard;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace Aurelia.DotNet.VSIX
{
    public class ProjectWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            // waiting for CLI unattended mode to finish then this will auto generate the dist folder for us in multiple project templates
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            var form = new ProjectWizardForm();
            form.ShowDialog();
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
