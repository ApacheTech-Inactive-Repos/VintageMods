using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Vintagestory.API.Util;

namespace VintageMods.Core.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class DotNetExtensions
    {
        #region Strings

        /// <summary>
        ///     Returns a default string, if a specified string is <see langword="null" />, empty, or consists only of white-space
        ///     characters.
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

        /// <summary>
        ///     Determines whether the string instance contains any of the specified strings.
        /// </summary>
        /// <param name="str">The original string.</param>
        /// <param name="values">The list of strings to compare.</param>
        /// <returns>true if <paramref name="values">value</paramref> matches the beginning of this string; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values">value</paramref> is null.</exception>
        public static bool ContainsAny(this string str, IEnumerable<string> values)
        {
            return values.Any(str.Contains);
        }

        /// <summary>
        ///     Determines whether the string instance contains any of the specified strings.
        /// </summary>
        /// <param name="str">The original string.</param>
        /// <param name="values">The list of strings to compare.</param>
        /// <returns>true if <paramref name="values">value</paramref> matches the beginning of this string; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values">value</paramref> is null.</exception>
        public static bool ContainsAny(this string str, params string[] values)
        {
            return values.Any(str.Contains);
        }

        /// <summary>
        ///     Converts a number to its ordinal string representation.
        /// </summary>
        /// <param name="num">The number to convert.</param>
        /// <returns>A string that represents the number, in ordinal form.</returns>
        public static string ToOrdinal(this int num)
        {
            return (num % 100) switch
            {
                11 => num.ToString("#,###0") + "th",
                12 => num.ToString("#,###0") + "th",
                13 => num.ToString("#,###0") + "th",
                _ => (num % 10) switch
                {
                    1 => num.ToString("#,###0") + "st",
                    2 => num.ToString("#,###0") + "nd",
                    3 => num.ToString("#,###0") + "rd",
                    _ => num.ToString("#,###0") + "th"
                }
            };
        }

        #endregion

        #region Enums

        /// <summary>
        ///     Gets the description for the enum member, decorated with a DescriptionAttribute.
        /// </summary>
        public static string GetDescription(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        ///     Gets the description for the enum member, decorated with a DescriptionAttribute.
        /// </summary>
        public static int Count<T>(this T en) where T : System.Enum
        {
            return typeof(T).GetEnumNames().Length;
        }

        #endregion

        #region MEF Composition

        public static IEnumerable<Assembly> GetAssembliesWithExports(this CompositionContainer container)
        {
            return container.Catalog?.Parts
                .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                .Distinct()
                .ToList();
        }

        #endregion

        #region Collections

        /// <summary>Returns the first element of a sequence, or null if the sequence contains no elements.</summary>
        /// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the first element of.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source" /> is <see langword="null" />.
        /// </exception>
        /// <returns>
        ///     <see langword="null" /> if <paramref name="source" /> is empty; otherwise, the first element in
        ///     <paramref name="source" />.
        /// </returns>
        [CanBeNull]
        public static TSource FirstOrNull<TSource>(this IEnumerable<TSource> source) where TSource : class
        {
            return source.DefaultIfEmpty(null).FirstOrDefault();
        }

        /// <summary>Returns the first element of the sequence that satisfies a condition or null if no such element is found.</summary>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <returns>
        ///     <see langword="null" /> if <paramref name="source" /> is empty or if no element passes the test specified by
        ///     <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test
        ///     specified by <paramref name="predicate" />.
        /// </returns>
        [CanBeNull]
        public static TSource FirstOrNull<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            where TSource : class
        {
            return source.DefaultIfEmpty(null).FirstOrDefault(predicate);
        }

        /// <summary>
        ///     Adds an item to the <see cref="ICollection{TItem}" />, if it not already present in the collection.
        /// </summary>
        /// <typeparam name="TItem">The type of the elements of <paramref name="item" />.</typeparam>
        /// <param name="collection">The collection to add the item to.</param>
        /// <param name="item">The item to add.</param>
        /// <returns><c>true</c> if the item was added to collection, <c>false</c> otherwise.</returns>
        public static bool AddIfNotPresent<TItem>(this ICollection<TItem> collection, TItem item)
        {
            var contains = collection.Contains(item);
            if (!contains) collection.Add(item);
            return !contains;
        }


        #endregion

        #region Assemblies

        public static IEnumerable<T> GetEnumerableOfType<T>(this Assembly assembly, params object[] constructorArgs)
            where T : class, IComparable<T>
        {
            var objects = assembly
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))
                .Select(type => (T)Activator.CreateInstance(type, constructorArgs))
                .ToList();
            objects.Sort();

            return objects;
        }

        public static IEnumerable<Type> GetDerivedTypes<T>(this T attribute, Assembly assembly) where T : Attribute
        {
            return assembly.GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(T), true).Length > 0);
        }

        public static IEnumerable<Type> GetTypesWithAttribute<T>(this Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(T), true).Length > 0);
        }

        #endregion

        #region Objects

        private static readonly ConditionalWeakTable<object, object> ExtendedData = new();

        /// <summary>
        ///     Gets a dynamic collection of properties associated with an object instance, with a lifetime scoped to the lifetime
        ///     of the object
        /// </summary>
        /// <param name="obj">The object the properties are associated with.</param>
        /// <returns>A dynamic collection of properties associated with an object instance.</returns>
        public static dynamic DynamicProperties(this object obj)
        {
            return ExtendedData.GetValue(obj, _ => new ExpandoObject());
        }

        /// <summary>
        ///     Dynamically casts the object instance to a specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to cast to.</typeparam>
        /// <param name="obj">The instance to cast.</param>
        /// <returns>An instance of Type <typeparamref name="T" />.</returns>
        public static T As<T>(this object obj)
        {
            return (Type.GetTypeCode(typeof(T)) is TypeCode.DateTime or TypeCode.DBNull or TypeCode.Empty)
                ? throw new ArgumentOutOfRangeException(nameof(T),
                    "Objects of this TypeCode cannot be cast to, dynamically.")
                : (T)obj;
        }

        /// <summary>
        ///     A close approximation to Visual Basic's "With" keyword, that allows
        ///     batch setting of Properties, without needing to initialise the object.
        /// </summary>
        /// <typeparam name="T">The type of object to work with.</typeparam>
        /// <param name="item">The item to work with.</param>
        /// <param name="work">The work to be done.</param>
        public static void With<T>(this T item, Action<T> work)
        {
            work(item);
        }

        #endregion
    }
}