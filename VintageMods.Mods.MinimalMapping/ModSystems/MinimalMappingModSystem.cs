using System.Linq;
using System.Text;
using HarmonyLib;
using VintageMods.Core.Common.ModSystems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedType.Global

namespace VintageMods.Mods.MinimalMapping.ModSystems
{
    internal class MinimalMappingModSystem : UniversalModSystem
    {
        private const string PatchCode = "VintageMods.MinimalMapping.HarmonyPatches";

        private readonly Harmony _harmony = new Harmony(PatchCode);

        private bool _patched;

        private void ApplyPatches(ICoreAPI api)
        {
            if (_patched) return;
            _harmony.PatchAll();
            var sb = new StringBuilder($"Harmony Patched Methods ({PatchCode}): ");
            sb.Append(string.Join(", ", _harmony.GetPatchedMethods().Select(p => p.Name)));
            api.Logger.Debug(sb.ToString());
            _patched = true;
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            ApplyPatches(api);

            api.Event.LevelFinalize += () =>
            {
                ClientSettings.ShowCoordinateHud = false;
                api.World.Config.SetBool("allowMap", true);
                api.Settings.Bool["showMinimapHud"] = true;

                api.Input.HotKeys.Remove("worldmapdialog");
                api.Input.HotKeys.Remove("coordinateshud");
            };
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            ApplyPatches(api);
        }

        public override void Dispose()
        {
            _harmony.UnpatchAll(PatchCode);
        }
    }
}
