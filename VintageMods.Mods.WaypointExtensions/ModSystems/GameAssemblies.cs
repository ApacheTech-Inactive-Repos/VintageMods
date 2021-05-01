using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Mods.WaypointExtensions.ModSystems
{
    public static class GameAssemblies
    {
        public static Assembly VSEssentials => typeof(BlockEntityGeneric).Assembly;
        public static Assembly VSSurvivalMod => typeof(BlockAnvil).Assembly;
        public static Assembly VSCreativeMod => typeof(BlockCommand).Assembly;
        public static Assembly VintagestoryAPI => typeof(ICoreClientAPI).Assembly;
        public static Assembly VintagestoryLib => typeof(ClientMain).Assembly;

        public static IReadOnlyList<Assembly> All { get; } = new List<Assembly>
        {
            VSEssentials, VSSurvivalMod, VSCreativeMod, VintagestoryAPI, VintagestoryLib
        };

        public static Type FindType(this Assembly assembly, string typeName)
        {
            return AccessTools.GetTypesFromAssembly(assembly)
                .FirstOrNull(t => t.Name == typeName);
        }

        public static Type FindType(string typeName)
        {
            return All.Select(assembly => assembly.FindType(typeName)).FirstOrNull();
        }
    }

    public static class Extensions
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

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}