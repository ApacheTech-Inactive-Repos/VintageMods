using VintageMods.Core.MemoryAdaptor.Marshaling;

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