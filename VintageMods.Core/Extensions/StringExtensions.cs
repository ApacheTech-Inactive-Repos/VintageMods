using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Vintagestory.API.Util;

namespace VintageMods.Core.Extensions
{
    /// <summary>
    ///     Contains extension methods for working with strings.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class StringExtensions
    {
        /// <summary>
        ///     Returns a default string, if a specified string is <see langword="null" />, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="defaultString">The default string.</param>
        public static string IfNullOrWhitespace(this string str, string defaultString)
        {
            return string.IsNullOrWhiteSpace(str) ? defaultString : str;
        }

        /// <summary>
        ///     Returns a default string, if a specified string is <see langword="null" />, or empty.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="defaultString">The default string.</param>
        public static string IfNullOrEmpty(this string str, string defaultString)
        {
            return string.IsNullOrEmpty(str) ? defaultString : str;
        }

        /// <summary>
        ///     Determines whether the beginning of this string instance matches any of the specified strings.
        /// </summary>
        /// <param name="str">The original string.</param>
        /// <param name="values">The list of strings to compare.</param>
        /// <returns>true if <paramref name="values">value</paramref> matches the beginning of this string; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values">value</paramref> is null.</exception>
        public static bool StartsWithAny(this string str, IEnumerable<string> values)
        {
            return values.Any(str.StartsWithFast);
        }

        /// <summary>
        ///     Determines whether the beginning of this string instance matches any of the specified strings.
        /// </summary>
        /// <param name="str">The original string.</param>
        /// <param name="values">The list of strings to compare.</param>
        /// <returns>true if <paramref name="values">value</paramref> matches the beginning of this string; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values">value</paramref> is null.</exception>
        public static bool StartsWithAny(this string str, params string[] values)
        {
            return values.Any(str.StartsWithFast);

        }
    }
}