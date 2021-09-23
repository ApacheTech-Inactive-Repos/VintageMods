using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StaticMemberInGenericType

namespace VintageMods.Core.Primitives
{
    public class AssetEnum<T> : StringEnum<T> where T : AssetEnum<T>, new()
    {
        public AssetEnum(string prefix)
        {
            Prefix = prefix;
        }

        public static string Prefix { get; private set; }

        public static T Any { get; } = Create($"{Prefix}*");

        public static List<T> ToList()
        {
            return typeof(T).GetProperties()
                .Where(prop => prop.PropertyType == typeof(T))
                .Where(prop => prop.Name != "Any")
                .Select(prop => (T) prop.GetValue(null, null)).ToList();
        }

        public static List<string> ToStringList()
        {
            return typeof(T).GetProperties()
                .Where(prop => prop.PropertyType == typeof(T))
                .Where(prop => prop.Name != "Any")
                .Select(prop => (string) prop.GetValue(null, null)).ToList();
        }
    }
}