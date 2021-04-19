using System.Collections.Generic;

namespace VintageMods.Core.MemoryAdaptor.Patterns
{
    public interface IMemoryPattern
    {
        int Offset { get; }
        MemoryPatternType PatternType { get; }
        IList<byte> GetBytes();
        string GetMask();
        PatternScannerAlgorithm Algorithm { get; }
    }
}