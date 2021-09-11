namespace VintageMods.Core.Extensions
{
    public static class EnumEx
    {
        /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.</summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type <paramref name="enumType">enumType</paramref> whose value is represented by <paramref name="value">value</paramref>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="enumType">enumType</paramref> or <paramref name="value">value</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="enumType">enumType</paramref> is not an <see cref="T:System.Enum"></see>.   -or-  <paramref name="value">value</paramref> is either an empty string ("") or only contains white space.   -or-  <paramref name="value">value</paramref> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="T:System.OverflowException"><paramref name="value">value</paramref> is outside the range of the underlying type of <paramref name="enumType">enumType</paramref>.</exception>

        public static TEnum Parse<TEnum>(string value, bool ignoreCase = false) where TEnum : System.Enum
        {
            return (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
    }
}