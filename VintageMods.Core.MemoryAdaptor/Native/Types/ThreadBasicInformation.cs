using System.Runtime.InteropServices;

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Structure containing basic information about a thread.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ThreadBasicInformation
    {
        /// <summary>
        ///     the exit status.
        /// </summary>
        public uint ExitStatus;

        /// <summary>
        ///     the exit status.
        /// </summary>
        public uint Padding;

        /// <summary>
        ///     The base address of Thread Environment Block.
        /// </summary>
        public ulong TebBaseAdress;

        /// <summary>
        ///     The process id which owns the thread.
        /// </summary>
        public ClientId ClientId;

        /// <summary>
        ///     The affinity mask.
        /// </summary>
        public ulong AffinityMask;

        /// <summary>
        ///     The priority.
        /// </summary>
        public uint Priority;

        /// <summary>
        ///     The base priority.
        /// </summary>
        public uint BasePriority;
    }
}