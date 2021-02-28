using System;
using System.Collections.Generic;
using VintageMods.WaypointExtensions.Model;

namespace VintageMods.WaypointExtensions.Extensions
{
    public static class SortedDictionaryEx
    {
        public static void AddRange(this SortedDictionary<string, WaypointInfoModel> dict, List<WaypointInfoModel> list)
        {
            list.ForEach(record =>
            {
                foreach (var syntax in record.Syntax)
                    try
                    {
                        dict.Add(syntax, record);
                    }
                    catch (ArgumentException)
                    {
                        dict.Remove(syntax);
                        dict.Add(syntax, record);
                    }
            });
        }
    }
}