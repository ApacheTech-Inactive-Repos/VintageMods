using System;
using System.Runtime.InteropServices;

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Contains the flash status for a window and the number of times the system should flash the window.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FlashInfo
    {
        public int Size;
        public IntPtr Hwnd;
        public FlashWindowFlags Flags;
        public int Count;
        public int Timeout;
    }
}