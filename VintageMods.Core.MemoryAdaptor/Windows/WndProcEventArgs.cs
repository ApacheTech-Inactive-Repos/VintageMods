using System;

namespace VintageMods.Core.MemoryAdaptor.Windows
{
    public class WndProcEventArgs : EventArgs
    {
        public WndProcEventArgs(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            Hwnd = hwnd;
            Msg = msg;
            WParam = wParam;
            LParam = lParam;
        }

        public IntPtr Hwnd { get; }

        public int Msg { get; }

        public IntPtr WParam { get; }

        public IntPtr LParam { get; }
    }
}