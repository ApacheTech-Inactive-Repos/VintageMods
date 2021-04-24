using System;

namespace VintageMods.Core.MemoryAdaptor.Windows
{
    public delegate IntPtr WindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
}