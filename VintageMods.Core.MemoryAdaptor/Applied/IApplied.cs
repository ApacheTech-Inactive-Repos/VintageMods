using VintageMods.Core.MemoryAdaptor.Marshalling;

namespace VintageMods.Core.MemoryAdaptor.Applied
{
    public interface IApplied : IDisposableState
    {
        string Identifier { get; }
        bool IsEnabled { get; }
        void Disable();
        void Enable();
    }
}