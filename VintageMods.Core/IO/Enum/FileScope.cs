using System;
using System.ComponentModel;
using System.Text;
using JetBrains.Annotations;

namespace VintageMods.Core.IO.Enum
{
    /// <summary>
    ///     Specifies the scope of a file saved to the user's game folder.
    /// </summary>
    public enum FileScope
    {
        /// <summary>
        ///     Denotes that a file is held in global scope, for all multi-player and single-player worlds.
        /// </summary>
        [Description("Global File")] Global,

        /// <summary>
        ///     Denotes that a file is created for each world a player enters.
        /// </summary>
        [Description("Per World File")] World
    }

    public static class FileScopeExtensions
    {
        /// <summary>
        ///     Converts the value of this FileScope enum to its equivalent string representation.
        /// </summary>
        /// <param name="scope">The FileScope enum value to convert.</param>
        [NotNull]
        public static string FastToString(this FileScope scope)
        {
            return scope switch
            {
                FileScope.Global => nameof(FileScope.Global),
                FileScope.World => nameof(FileScope.World),
                _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
            };
        }
    }
}