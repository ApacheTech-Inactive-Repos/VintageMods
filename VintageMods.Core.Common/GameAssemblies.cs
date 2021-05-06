using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using VintageMods.Core.Common.Extensions;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Core.Common
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
}