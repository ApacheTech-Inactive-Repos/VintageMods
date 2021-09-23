using System;
using System.Runtime.InteropServices;

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation64
    {
        /// <summary>
        ///     A pointer to the base address of the region of pages.
        /// </summary>
        public IntPtr BaseAddress;

        /// <summary>
        ///     A pointer to the base address of a range of pages allocated by the VirtualAlloc function. The page pointed to by
        ///     the BaseAddress member is contained within this allocation range.
        /// </summary>
        public IntPtr AllocationBase;

        /// <summary>
        ///     The memory protection option when the region was initially allocated. This member can be one of the memory
        ///     protection constants or 0 if the caller does not have access.
        /// </summary>
        public MemoryProtectionFlags AllocationProtect;

        /// <summary>
        ///     Required in the 64 bit struct. Blame Windows.
        /// </summary>
        public uint __alignment1;

        /// <summary>
        ///     The size of the region beginning at the base address in which all pages have identical attributes, in bytes.
        /// </summary>
        public long RegionSize;

        /// <summary>
        ///     The state of the pages in the region.
        /// </summary>
        public MemoryStateFlags State;

        /// <summary>
        ///     The access protection of the pages in the region. This member is one of the values listed for the AllocationProtect
        ///     member.
        /// </summary>
        public MemoryProtectionFlags Protect;

        /// <summary>
        ///     The type of pages in the region.
        /// </summary>
        public MemoryTypeFlags Type;

        /// <summary>
        ///     Required in the 64 bit struct. Blame Windows.
        /// </summary>
        public uint __alignment2;
    }
}