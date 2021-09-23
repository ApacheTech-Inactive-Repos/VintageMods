using System.Runtime.InteropServices;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InvalidXmlDocComment
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     Represents a thread context.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ThreadContext
    {
        /// <summary>
        ///     Determines which registers are returned or set when using <see cref="NativeMethods.GetThreadContext" /> or
        ///     <see cref="NativeMethods.SetThreadContext" />.
        ///     If the context record is used as an INPUT parameter, then for each portion of the context record controlled by a
        ///     flag whose value is set, it is assumed that portion of the
        ///     context record contains valid context. If the context record is being used to modify a threads context, then only
        ///     that portion of the threads context will be modified.
        ///     If the context record is used as an INPUT/OUTPUT parameter to capture the context of a thread, then only those
        ///     portions of the thread's context corresponding to set flags will be returned.
        ///     The context record is never used as an OUTPUT only parameter.
        /// </summary>
        public ThreadContextFlags ContextFlags;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public int Dr0;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public int Dr1;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public int Dr2;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public int Dr3;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public int Dr6;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public int Dr7;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.FloatingPoint" />.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)] public FloatingSaveArea FloatingSave;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public int SegGs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public int SegFs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public int SegEs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public int SegDs;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public int Edi;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public int Esi;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public int Ebx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public int Edx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public int Ecx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public int Eax;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public int Ebp;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public int Eip;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public int SegCs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public int EFlags;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public int Esp;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public int SegSs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.ExtendedRegisters" />.
        ///     The format and contexts are processor specific.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] ExtendedRegisters;
    }
}