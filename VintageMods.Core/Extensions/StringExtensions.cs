namespace VintageMods.Core.Extensions
{
    public static class StringExtensions
    {
        public static string IfNullOrWhitespace(this string str, string defaultString)
        {
            return string.IsNullOrWhiteSpace(str) ? defaultString : str;
        }
        public static string IfNullOrEmpty(this string str, string defaultString)
        {
            return string.IsNullOrEmpty(str) ? defaultString : str;
        }
    }
}