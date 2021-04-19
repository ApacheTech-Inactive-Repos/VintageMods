namespace VintageMods.Core.MemoryAdaptor.Patterns
{
    public interface IPatternScanner
    {
        PatternScanResult Find(IMemoryPattern pattern);
    }
}