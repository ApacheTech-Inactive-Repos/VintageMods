using System;
using System.Collections.Generic;
using System.Linq;

namespace VintageMods.Core.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static T FirstOrNull<T>(this IEnumerable<T> values) where T : class
        {
            return values.DefaultIfEmpty(null).FirstOrDefault();
        }
        public static T FirstOrNull<T>(this IEnumerable<T> values, Func<T, bool> predicate) where T : class
        {
            return values.DefaultIfEmpty(null).FirstOrDefault(predicate);
        }
    }
}