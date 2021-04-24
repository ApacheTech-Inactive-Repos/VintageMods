using System.Runtime.InteropServices;

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Returned if <see cref="ThreadContextFlags.FloatingPoint" /> flag is set.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FloatingSaveArea
    {
        public int ControlWord;
        public int StatusWord;
        public int TagWord;
        public int ErrorOffset;
        public int ErrorSelector;
        public int DataOffset;
        public int DataSelector;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)] public byte[] RegisterArea;
        public int Cr0NpxState;
    }
}