namespace VintageMods.Core.MemoryAdaptor.Utilities
{
    //Original C++ implementation by DarthTon. Ported to C# by gir489.
    public class BoyerMooreHorspool
    {
        private const byte WildCard = 0x00;

        private static int[] BuildBadCharTable(byte[] pPattern)
        {
            int idx;
            var last = pPattern.Length - 1;
            var badShift = new int[256];

            // Get last wildcard position
            for (idx = last; idx > 0 && pPattern[idx] != WildCard; --idx)
            {
            }

            var diff = last - idx;
            if (diff == 0)
                diff = 1;

            // Prepare shift table
            for (idx = 0; idx <= 255; ++idx)
                badShift[idx] = diff;
            for (idx = last - diff; idx < last; ++idx)
                badShift[pPattern[idx]] = last - idx;
            return badShift;
        }

        public static int IndexOf(byte[] buffer, byte[] pattern)
        {
            if (pattern.Length > buffer.Length) return -1;
            var badShift = BuildBadCharTable(pattern);
            var offset = 0;
            var last = pattern.Length - 1;
            var maxoffset = buffer.Length - pattern.Length;
            while (offset <= maxoffset)
            {
                int position;
                for (position = last;
                    pattern[position] == buffer[position + offset] || pattern[position] == WildCard;
                    position--)
                    if (position == 0)
                        return offset;
                offset += badShift[buffer[offset + last]];
            }

            return -1;
        }
    }
}