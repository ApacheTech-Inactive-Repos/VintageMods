using System.Linq;
using VintageMods.Core.MemoryAdaptor.Modules;
using VintageMods.Core.MemoryAdaptor.Patterns;

namespace VintageMods.Core.MemoryAdaptor.Utilities
{
    public static class Naive
    {
        public static int GetIndexOf(IMemoryPattern pattern, byte[] data, IProcessModule module)
        {
            var patternData = data;
            var patternDataLength = patternData.Length;

            for (var offset = 0; offset < patternDataLength; offset++)
            {
                if (
                    pattern.GetMask()
                        .Where((m, b) => m == 'x' && pattern.GetBytes()[b] != patternData[b + offset])
                        .Any())
                    continue;

                return offset;
            }

            return -1;
        }
    }
}