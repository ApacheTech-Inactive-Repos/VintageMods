namespace VintageMods.Core.Extensions
{
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
    }
}