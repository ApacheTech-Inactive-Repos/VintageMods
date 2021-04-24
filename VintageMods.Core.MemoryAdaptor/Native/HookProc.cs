using System;

namespace VintageMods.Core.MemoryAdaptor.Native
{
    public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
}