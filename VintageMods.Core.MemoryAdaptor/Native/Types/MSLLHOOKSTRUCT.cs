using System;
using System.Runtime.InteropServices;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable InvalidXmlDocComment
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CommentTypo

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public Point Point { get; set; }
        public int MouseData { get; set; }
        public int Flags { get; set; }
        public int Time { get; set; }
        public IntPtr DwExtraInfo { get; set; }
    }
}