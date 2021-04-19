using System;
using System.Collections.Generic;

namespace VintageMods.Mods.WaypointExtensions.Extensions
{
    public static class SortedDictionaryEx
    {
        /// <summary>
        ///     Adds all syntax entries from within a syntax list, into a sorted dictionary.
        ///     Includes a work around the fact that keys within Sorted Dictionaries cannot normally be overwritten.
        /// </summary>
        /// <param name="dict">The dictionary to save the syntax list to.</param>
        /// <param name="list">The list of syntax options to add.</param>
        /// <param name="predicate">The data member to search by.</param>
        public static void AddRange<TK, TV>(this SortedDictionary<TK, TV> dict, IEnumerable<TV> list, Func<TV, IEnumerable<TK>> predicate)
        {
            foreach (var record in list)
            {
                foreach (var key in predicate(record))
                {
                    try
                    { 
                        dict.Add(key, record);
                    }
                    catch (ArgumentException)
                    {
                        dict.Remove(key);
                        dict.Add(key, record);
                    }
                }
            }
        }
    }
}