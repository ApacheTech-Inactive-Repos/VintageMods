using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace VintageMods.Core.Common.Primitives
{
    public abstract class StringEnum<T> : IEquatable<T> where T : StringEnum<T>, new()
    {
        protected string Value;

        private static readonly Dictionary<string, T> ValueDict = new Dictionary<string, T>();

        protected static T Create(string value)
        {
            if (value == null) return default;
            var obj1 = new T { Value = value };
            var obj2 = obj1;
            ValueDict.Add(value, obj2);
            return obj2;
        }

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