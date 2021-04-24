﻿using System;

// ReSharper disable InvalidXmlDocComment
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Thread creation options list.
    /// </summary>
    [Flags]
    public enum ThreadCreationFlags
    {
        /// <summary>
        ///     The thread runs immediately after creation.
        /// </summary>
        Run = 0x0,

        /// <summary>
        ///     The thread is created in a suspended state, and does not run until the <see cref="NativeMethods.ResumeThread" />
        ///     function is called.
        /// </summary>
        Suspended = 0x04,

        /// <summary>
        ///     The dwStackSize parameter specifies the initial reserve size of the stack. If this flag is not specified,
        ///     dwStackSize specifies the commit size.
        /// </summary>
        StackSizeParamIsAReservation = 0x10000
    }
}