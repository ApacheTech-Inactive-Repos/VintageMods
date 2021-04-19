using VintageMods.Core.MemoryAdaptor.Marshaling;

namespace VintageMods.Core.MemoryAdaptor.Memory
{
    public interface IAllocatedMemory : IPointer, IDisposableState
    {
        bool IsAllocated { get; }
        int Size { get; }
        string Identifier { get; }
    }
}