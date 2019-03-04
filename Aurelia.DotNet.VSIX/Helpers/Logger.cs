using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Logger
{
    private static string _name;
    private static IVsOutputWindowPane _windowPane;
    private static IVsOutputWindow _output;

    public static void Initialize(IServiceProvider provider, string name)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        _output = (IVsOutputWindow)provider.GetService(typeof(SVsOutputWindow));
        Assumes.Present(_output);
        _name = name;
    }

    public static void Log(object message)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        try
        {
            if (PaneExistsElseCreate())
            {
                _windowPane.OutputString(DateTime.Now.ToString() + ": " + message + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            Debug.Write(ex);
        }
    }

    private static bool PaneExistsElseCreate()
    {
        ThreadHelper.ThrowIfNotOnUIThread(); 
        if(_windowPane != null) { return true; }
        var guid = Guid.NewGuid();
        _output.CreatePane(ref guid, _name, 1, 1);
        _output.GetPane(ref guid, out _windowPane);
        return _windowPane != null;
    }
}
