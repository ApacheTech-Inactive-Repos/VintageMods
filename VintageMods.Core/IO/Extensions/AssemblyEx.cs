using System;
using JetBrains.Annotations;

namespace VintageMods.Core.IO.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class AssemblyEx
    {
        /// <summary>
        ///     De-obfuscation of the game's in-built LoadLibrary function.
        /// </summary>
        public static IntPtr LoadLibrary(string lpParam)
        {
            return _ISiSTzGmXXpAqo3PzdxdVD1bClZ._ZiXYmH0gIJ0XiHUp5LNJEsvc1Wb(lpParam);
        }

        /// <summary>
        ///     De-obfuscation of the game's in-built SetDllDirectory function.
        /// </summary>
        public static bool SetDllDirectory(string lpParam)
        {
            return _ISiSTzGmXXpAqo3PzdxdVD1bClZ._L8vyKKkqrTz7yRZnop56s6nrCNC(lpParam);
        }
    }
}