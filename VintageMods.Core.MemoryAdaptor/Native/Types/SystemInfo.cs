using System;
using System.Runtime.InteropServices;

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemInfo
    {
        public ProcessorArchitecture ProcessorArchitecture; // WORD
        public int PageSize; // DWORD
        public IntPtr MinimumApplicationAddress; // (long)void*
        public IntPtr MaximumApplicationAddress; // (long)void*
        public IntPtr ActiveProcessorMask; // DWORD*
        public int NumberOfProcessors; // DWORD (WTF)
        public int ProcessorType; // DWORD
        public int AllocationGranularity; // DWORD
        public ushort ProcessorLevel; // WORD
        public ushort ProcessorRevision; // WORD
    }
}