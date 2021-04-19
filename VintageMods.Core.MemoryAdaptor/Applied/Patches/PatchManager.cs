using System;
using VintageMods.Core.MemoryAdaptor.Memory;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.MemoryAdaptor.Applied.Patches
{
    /// <summary>
    ///     A manager class to handle memory patches.
    ///     <remarks>All credits to Apoc.</remarks>
    /// </summary>
    public class PatchManager : ComplexAppliedManager<Patch>, IDisposable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PatchManager" /> class.
        /// </summary>
        /// <param name="processMemory">The process memory.</param>
        public PatchManager(IMemory processMemory)
        {
            MemoryBase = processMemory;
        }

        /// <summary>
        ///     The reference of the <see cref="MemoryBase" /> object.
        ///     <remarks>This value is invalid if the manager was created for the <see cref="ProcessSharp" /> class.</remarks>
        /// </summary>
        protected IMemory MemoryBase { get; }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            RemoveAll();
        }

        /// <summary>
        ///     Creates a new <see cref="Patch" /> at the specified address.
        /// </summary>
        /// <param name="address">The address to begin the patch.</param>
        /// <param name="patchWith">The bytes to be written as the patch.</param>
        /// <param name="name">The name of the patch.</param>
        /// <returns>A patch object that exposes the required methods to apply and remove the patch.</returns>
        public Patch Create(IntPtr address, byte[] patchWith, string name)
        {
            if (InternalItems.ContainsKey(name))
                return InternalItems[name];
            InternalItems.Add(name, new Patch(address, patchWith, name, MemoryBase));
            return InternalItems[name];
        }

        /// <summary>
        ///     Creates a new <see cref="Patch" /> at the specified address, and applies it.
        /// </summary>
        /// <param name="address">The address to begin the patch.</param>
        /// <param name="patchWith">The bytes to be written as the patch.</param>
        /// <param name="name">The name of the patch.</param>
        /// <returns>A patch object that exposes the required methods to apply and remove the patch.</returns>
        public Patch CreateAndApply(IntPtr address, byte[] patchWith, string name)
        {
            Create(address, patchWith, name);
            InternalItems[name].Enable();
            return InternalItems[name];
        }
    }
}