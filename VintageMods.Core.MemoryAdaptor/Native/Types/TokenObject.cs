using System;

// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    [Flags]
    public enum TokenObject
    {
        TOKEN_QUERY = 0x0008,
        TOKEN_QUERY_SOURCE = 0x0010,
        TOKEN_ADJUST_PRIVILEGES = 0x0020
    }
}