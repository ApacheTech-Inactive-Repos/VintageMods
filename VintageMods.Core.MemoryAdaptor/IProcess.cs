using System;
using System.Diagnostics;
using VintageMods.Core.MemoryAdaptor.Memory;
using VintageMods.Core.MemoryAdaptor.Modules;
using VintageMods.Core.MemoryAdaptor.Native.Types;
using VintageMods.Core.MemoryAdaptor.Threads;
using VintageMods.Core.MemoryAdaptor.Windows;

namespace VintageMods.Core.MemoryAdaptor
{
    /// <summary>
    ///     A class that offers several tools to interact with a process.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IProcess : IDisposable
    {
        /// <summary>
        ///     Provide access to the opened process.
        /// </summary>
        Process Native { get; set; }

        /// <summary>
        ///     The process handle opened with all rights.
        /// </summary>
        SafeMemoryHandle Handle { get; set; }

        /// <summary>
        ///     Class for reading and writing memory.
        /// </summary>
        IMemory Memory { get; set; }

        /// <summary>
        ///     Factory for manipulating threads.
        /// </summary>
        IThreadFactory ThreadFactory { get; set; }

        /// <summary>
        ///     Factory for manipulating modules and libraries.
        /// </summary>
        IModuleFactory ModuleFactory { get; set; }

        /// <summary>
        ///     Factory for manipulating memory space.
        /// </summary>
        IMemoryFactory MemoryFactory { get; set; }

        /// <summary>
        ///     Factory for manipulating windows.
        /// </summary>
        IWindowFactory WindowFactory { get; set; }

        /// <summary>
        ///     Gets the specified module in the process.
        /// </summary>
        /// <param name="moduleName">The name of module.</param>
        /// <returns><see cref="IProcessModule" />.</returns>
        IProcessModule this[string moduleName] { get; }

        /// <summary>
        ///     Gets a pointer to the specified address in the process.
        /// </summary>
        /// <param name="intPtr">The address pointed.</param>
        /// <returns>
        ///     <see cref="IPointer" />
        /// </returns>
        IPointer this[IntPtr intPtr] { get; }
    }
}