using System;
using System.Runtime.InteropServices;

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Sub-structure of ThreadBasicInformation
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ClientId
    {
        /// <summary>
        ///     The process id which owns the thread.
        /// </summary>
        public IntPtr UniqueProcess;

        /// <summary>
        ///     The thread id.
        /// </summary>
        public IntPtr UniqueThread;
    }
}