using System;

namespace VintageMods.Core.MemoryAdaptor.Threads
{
    public interface IFrozenThread : IDisposable
    {
        IRemoteThread Thread { get; }
    }
}