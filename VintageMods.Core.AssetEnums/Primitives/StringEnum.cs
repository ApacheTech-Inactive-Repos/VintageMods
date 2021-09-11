using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace VintageMods.Core.AssetEnum.Primitives
{
    /// <summary>
    ///     Class StringEnum.
    /// Implements the <see cref="System.IEquatable{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IEquatable{T}" />
    public abstract class StringEnum<T> : IEquatable<T> where T : StringEnum<T>, new()
    {
        /// <summary>
        ///     The value given to this StringEnum member.
        /// </summary>
        protected string Value;

        /// <summary>
        /// The value dictionary
        /// </summary>
        private static readonly Dictionary<string, T> ValueDict = new();

        /// <summary>
        ///     Creates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        protected static T Create(string value)
        {
            if (value == null) return default;
            var obj1 = new T { Value = value };
            ValueDict.Add(value, obj1);
            return obj1;
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="StringEnum{T}" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(StringEnum<T> enumValue)
        {
            return enumValue.Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator !=(StringEnum<T> o1, StringEnum<T> o2)
        {
            return ((object) o1 != null ? o1.Value : null) != ((object) o2 != null ? o2.Value : null);
        }

        public static bool operator ==(StringEnum<T> o1, StringEnum<T> o2)
        {
            return ((object) o1 != null ? o1.Value : null) == ((object) o2 != null ? o2.Value : null);
        }

        public override bool Equals(object other)
        {
            return Value.Equals((other as T)?.Value ?? other as string);
        }

        bool IEquatable<T>.Equals(T other)
        {
            return Value.Equals(other?.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static T Parse(string value, bool caseSensitive = true)
        {
            var obj = TryParse(value, caseSensitive);
            return !(obj == null)
                ? obj
                : throw new InvalidOperationException((value == null ? "null" : "'" + value + "'") +
                                                      " is not a valid " + typeof(T).Name);
        }

        public static T TryParse(string value, bool caseSensitive = true)
        {
            if (value == null)
                return default;
            if (ValueDict.Count == 0)
                RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
            if (!caseSensitive)
                return ValueDict.FirstOrDefault(f =>
                        f.Key.Equals(value, StringComparison.OrdinalIgnoreCase)).Value;
            return ValueDict.TryGetValue(value, out var obj) ? obj : default;
        }
    }
}