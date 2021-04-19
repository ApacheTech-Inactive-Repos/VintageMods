using System;
using VintageMods.Core.MemoryAdaptor.Memory;

namespace VintageMods.Core.MemoryAdaptor.Marshaling
{
    /// <summary>
    ///     Interface representing a value within the memory of a remote process.
    /// </summary>
    public interface IMarshalledValue : IDisposable
    {
        /// <summary>
        ///     The memory allocated where the value is fully written if needed. It can be unused.
        /// </summary>
        IAllocatedMemory Allocated { get; }

        /// <summary>
        ///     The reference of the value. It can be directly the value or a pointer.
        /// </summary>
        IntPtr Reference { get; }
    }
}