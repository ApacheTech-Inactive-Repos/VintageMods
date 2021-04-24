using System;
using VintageMods.Core.MemoryAdaptor.Marshaling;

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Structure containing basic information about a process.
    /// </summary>
    public struct ProcessBasicInformation
    {
        /// <summary>
        ///     The exit status.
        /// </summary>
        public int ExitStatus;

        /// <summary>
        ///     The base address of Process Environment Block.
        /// </summary>
        public IntPtr PebBaseAddress;

        /// <summary>
        ///     The affinity mask.
        /// </summary>
        public int AffinityMask;

        /// <summary>
        ///     The base priority.
        /// </summary>
        public int BasePriority;

        /// <summary>
        ///     The process id.
        /// </summary>
        public int ProcessId;

        /// <summary>
        ///     The process id of the parent process.
        /// </summary>
        public int InheritedFromUniqueProcessId;

        /// <summary>
        ///     The size of this structure.
        /// </summary>
        public int Size => MarshalCache<ProcessBasicInformation>.Size;
    }
}