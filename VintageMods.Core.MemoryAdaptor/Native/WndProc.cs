using System;

namespace VintageMods.Core.MemoryAdaptor.Native
{
    public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
}