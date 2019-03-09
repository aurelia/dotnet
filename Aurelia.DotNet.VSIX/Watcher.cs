#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Threading;
using Aurelia.DotNet;

namespace Aurelia.DotNet.VSIX
{
    public class Watcher : IVsSolutionEvents, IVsSolutionLoadEvents, IDisposable
    {
        uint mSolutionCookie;
        CancellationTokenSource mToken;

        private EnvDTE.WindowEvents mWindowEvents;
        private EnvDTE.DocumentEvents mDocumentEvents;
        private EnvDTE.ProjectItemsEvents mSolutionItemsEvents;

        public Watcher()
        {
            if (Instance.Solution != null)
            {
                Instance.Solution.AdviseSolutionEvents(this, out mSolutionCookie);
            }

            mDocumentEvents = Instance.DTE2.Events.DocumentEvents;
            mDocumentEvents.DocumentSaved += OnDocumentSaved;

            mSolutionItemsEvents = Instance.DTE2.Events.SolutionItemsEvents;
            mSolutionItemsEvents.ItemAdded += OnItemAdded;
            mSolutionItemsEvents.ItemRemoved += OnItemRemoved;
        }

        public void Dispose()
        {
            if (Instance.Solution != null && mSolutionCookie != 0)
            {
                Instance.Solution.UnadviseSolutionEvents(mSolutionCookie);
            }
        }

        private void OnDocumentSaved(Document Document)
        {
            if (Document.Name.IsAureliaCliFile())
            {
                AureliaHelper.LoadAureliaCli(Document.Path);
            }
        }

        private void OnItemRemoved(ProjectItem ProjectItem)
        {
            if (ProjectItem.Name.IsAureliaCliFile())
            {
                AureliaHelper.AureliaCli = null;
            }
        }

        private void OnItemAdded(ProjectItem ProjectItem)
        {
            if (ProjectItem.Name.IsAureliaCliFile())
            {
                Helpers.DteHelpers.GetSelectionData(ProjectItem, out var targetFolder, out var project, out var target);
                AureliaHelper.LoadAureliaCli(Path.Combine(targetFolder, ProjectItem.Name));
            }

        }

        private Project getActiveProject()
        {
            Array projects = (Array)(Instance.DTE2.ActiveSolutionProjects ?? Instance.DTE2.Solution.SolutionBuild.StartupProjects);

            if (projects != null && projects.Length > 0)
            {
                if (projects.GetValue(0) is Project project)
                {
                    return project;
                }
            }

            Projects projs = Instance.DTE2.Solution.Projects;
            if (projs != null && projs.Count > 0)
            {
                return projs.Item(1);
            }
            return null;
        }

        public int OnBeforeOpenSolution(string pszSolutionFilename)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeBackgroundSolutionLoadBegins()
        {
            return VSConstants.S_OK;
        }

        public int OnQueryBackgroundLoadProjectBatch(out bool pfShouldDelayLoadToNextIdle)
        {
            pfShouldDelayLoadToNextIdle = false;
            return VSConstants.S_OK;
        }

        public int OnBeforeLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterBackgroundSolutionLoadComplete()
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            var project = getActiveProject();
            var path = project.FullName;
            AureliaHelper.LoadAureliaCliFromPath(path);
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

    }
}
