using System;

namespace VintageMods.Core.MemoryAdaptor.Modules
{
    public interface IProcessFunction
    {
        IntPtr BaseAddress { get; }
        string Name { get; }
        T GetDelegate<T>();
    }
}