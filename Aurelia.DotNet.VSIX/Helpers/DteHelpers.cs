using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Aurelia.DotNet.VSIX.Helpers
{
    public static class DteHelpers
    {
        public static void GetSelectionData(
                                           object automationObject,
                                           out string targetFolderPath,
                                           out string projectFolderPath,
                                           out string projectFullName)
        {
            targetFolderPath = "";
            projectFolderPath = "";
            projectFullName = "";
            var dte = (automationObject as DTE);
            if (dte.SelectedItems.Count > 0)
            {
                var selectedItem = dte.SelectedItems.Item(1);
                Project currentProject;
                if (selectedItem.Project != null)
                {
                    currentProject = selectedItem.Project;
                    projectFullName = currentProject.FullName;
                    projectFolderPath = Path.GetDirectoryName(currentProject.FileName);
                    targetFolderPath = projectFolderPath;
                }
                else if (selectedItem.ProjectItem != null)
                {
                    currentProject = selectedItem.ProjectItem.ContainingProject;
                    projectFullName = currentProject.FullName;
                    projectFolderPath = Path.GetDirectoryName(currentProject.FileName);
                    if (selectedItem.ProjectItem.FileCount > 0)
                    {
                        targetFolderPath = selectedItem.ProjectItem.FileNames[0];
                    }
                }
            }
        }

        public static T OpenDialog<T>(DTE2 dte) where T : System.Windows.Window, new()
        {
            var dialog = new T();
            var hwnd = new IntPtr(dte.MainWindow.HWnd);
            var window = (System.Windows.Window)HwndSource.FromHwnd(hwnd).RootVisual;
            dialog.Owner = window;
            return dialog;
        }

        public static T OpenDialog<T>(this DTE2 dte, T dialog) where T : System.Windows.Window
        {
            var hwnd = new IntPtr(dte.MainWindow.HWnd);
            var window = (System.Windows.Window)HwndSource.FromHwnd(hwnd).RootVisual;
            dialog.Owner = window;
            return dialog;
        }

        public static Project GetActiveProject(DTE2 dte)
        {
            try
            {
                if (dte.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0)
                {
                    return activeSolutionProjects.GetValue(0) as Project;
                }

                var doc = dte.ActiveDocument;

                if (string.IsNullOrEmpty(doc?.FullName ?? null)) { return null; }
                var item = dte.Solution?.FindProjectItem(doc.FullName);
                return item?.ContainingProject ?? null;
            }
            catch (Exception ex)
            {
                Logger.Log("Error getting the active project" + ex);
            }

            return null;
        }

        public static IWpfTextView GetCurentTextView()
        {
            var componentModel = GetComponentModel();
            if (componentModel == null) return null;
            var editorAdapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            return editorAdapter.GetWpfTextView(CurrentVsTextView);
        }

        public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));


        public static IVsTextView CurrentVsTextView
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var textManager = (IVsTextManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));

                ErrorHandler.ThrowOnFailure(textManager.GetActiveView(1, null, out IVsTextView activeView));
                return activeView;
            }
        }
    }
}
