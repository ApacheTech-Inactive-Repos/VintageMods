using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using VintageMods.Core.Extensions;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Core.Helpers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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

        /// <summary>
        ///     Scans for a specific type within one of the game's vanilla assemblies. Includes internal classes, and nested
        ///     private classes. It can then be instantiated via Harmony.
        /// </summary>
        /// <param name="assembly">The assembly to scan within.</param>
        /// <param name="typeName">The name of the type to scan for.</param>
        /// <returns>The Type definition of the object being scanned for.</returns>
        public static Type FindType(this Assembly assembly, string typeName)
        {
            return AccessTools.GetTypesFromAssembly(assembly).FirstOrNull(t => t.Name == typeName);
        }

        /// <summary>
        ///     Scans for a specific type within the game's vanilla assemblies. Includes internal classes, and nested private
        ///     classes. It can then be instantiated via Harmony.
        /// </summary>
        /// <param name="typeName">The name of the type to scan for.</param>
        /// <returns>The Type definition of the object being scanned for.</returns>
        public static Type FindType(string typeName)
        {
            return All.Select(assembly => assembly.FindType(typeName)).FirstOrNull();
        }
    }
}