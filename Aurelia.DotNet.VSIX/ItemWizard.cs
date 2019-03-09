using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Aurelia.DotNet;

namespace Aurelia.DotNet.VSIX
{

    public class ItemWizard : IWizard
    {
        private DTE dte;
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (projectItem.Kind.ToLower() != "folder") { return; }
            if (projectItem.Name != "ClientApp") { return; }
            //Client folder was generated lets generate aurelia now

        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            Helpers.DteHelpers.GetSelectionData(automationObject, out var targetFolderPath, out var projectFolderPath, out var projectFullName);
            var wiz = new DotNet.Wizard.ItemWizard();
            wiz.ShowDialog();
            string pascal = replacementsDictionary["$safeitemname$"].ToPascalCase();
            replacementsDictionary["$pascalName$"] = pascal;
            replacementsDictionary["$camelName$"] = pascal.LowerCaseFirstLetter();
            replacementsDictionary["kebabName"] = pascal.PascalToKebabCase();

        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
