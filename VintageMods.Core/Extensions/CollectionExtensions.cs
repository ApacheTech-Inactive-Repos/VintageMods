using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace VintageMods.Core.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>Returns the first element of a sequence, or null if the sequence contains no elements.</summary>
        /// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the first element of.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is <see langword="null" />.</exception>
        /// <returns>
        ///     <see langword="null" /> if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.
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
        /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
        /// <returns>
        ///     <see langword="null" /> if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.
        /// </returns>
        [CanBeNull]
        public static TSource FirstOrNull<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) where TSource : class
        {
            return source.DefaultIfEmpty(null).FirstOrDefault(predicate);
        }
    }
}