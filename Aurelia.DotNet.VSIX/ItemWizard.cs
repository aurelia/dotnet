using System.Collections.Generic;
using Aurelia.Dotnet.Wizard;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace Aurelia.DotNet.VSIX
{    

    public class ItemWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            //var form = new ItemWizardForm();
            //form.ShowDialog();
            string pascal = replacementsDictionary["$safeitemname$"].ToPascalCase();
            replacementsDictionary["$pascalName$"] = pascal;
            replacementsDictionary["$camelName$"] = pascal.LowerCaseFirstLetter();
            replacementsDictionary["$snakeName$"] = pascal.PascalToKebabCase();

        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
