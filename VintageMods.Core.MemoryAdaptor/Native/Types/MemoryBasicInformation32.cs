﻿using System;
using System.Runtime.InteropServices;
// ReSharper disable FieldCanBeMadeReadOnly.Global

// ReSharper disable InvalidXmlDocComment
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Contains information about a range of pages in the virtual address space of a process. The VirtualQuery and
    ///     <see cref="NativeMethods.VirtualQueryEx" /> functions use this structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation32
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
        ///     The size of the region beginning at the base address in which all pages have identical attributes, in bytes.
        /// </summary>
        public readonly int RegionSize;

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
    }
}