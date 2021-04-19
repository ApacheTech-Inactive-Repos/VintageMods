using System;
using System.Collections.Generic;

namespace VintageMods.Core.MemoryAdaptor.Windows
{
    public interface IWindowFactory : IDisposable
    {
        IEnumerable<IWindow> this[string windowTitle] { get; }

        IEnumerable<IWindow> ChildWindows { get; }
        IWindow MainWindow { get; set; }
        IEnumerable<IWindow> Windows { get; }

        IEnumerable<IWindow> GetWindowsByClassName(string className);
        IEnumerable<IWindow> GetWindowsByTitle(string windowTitle);
        IEnumerable<IWindow> GetWindowsByTitleContains(string windowTitle);
    }
}