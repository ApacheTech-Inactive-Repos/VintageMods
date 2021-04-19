using System.Text;
using HarmonyLib;
using VintageMods.Core.Client.ModSystems;
using VintageMods.Core.FileIO.Enum;
using VintageMods.Core.FileIO.Extensions;
using VintageMods.Mods.EnvironmentalTweaks.Config;
using VintageMods.Mods.EnvironmentalTweaks.HarmonyPatches;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace VintageMods.Mods.EnvironmentalTweaks.ModSystems
{
    internal class EnvTweaksModSystem : ClientSideModSystem
    {
        private const string PatchCode = "VintageMods.Mods.EnvironmentalTweaks";
        private readonly Harmony _harmonyInstance = new Harmony(PatchCode);


        public override void StartClientSide(ICoreClientAPI api)
        {
            var settingsFile = api.RegisterFileManager("EnvTweaks").RegisterConfigFile("EnvTweaks.config.json", FileScope.Global);
            EnvTweaksPatches.Api = api;
            EnvTweaksPatches.Settings = settingsFile.ParseJsonAsObject<ModSettings>();

            _harmonyInstance.PatchAll();
            var builder = new StringBuilder("Harmony Patched Methods: ");
            foreach (var val in _harmonyInstance.GetPatchedMethods())
            {
                builder.Append(val.Name + ", ");
            }
            api.Logger.Notification(builder.ToString());


            api.RegisterCommand("EnvTweaks", 
                "Change settings for Environmental Tweaks.", 
                "[lightning|rain|hail|snow|sounds|shake] [on|off]", 
                (id, args) =>
            {
                var option = args.PopWord();
                var state = args.PopWord("off").ToLowerInvariant() == "on";
                switch (option)
                {
                    case "lightning":
                        EnvTweaksPatches.Settings.AllowLightning = state;
                        break;

                    case "sounds":
                        EnvTweaksPatches.Settings.AllowWeatherSounds = state;
                        break;

                    case "rain":
                        EnvTweaksPatches.Settings.AllowRain = state;
                        break;

                    case "hail":
                        EnvTweaksPatches.Settings.AllowHail = state;
                        break;

                    case "snow":
                        EnvTweaksPatches.Settings.AllowSnow = state;
                        break;

                    case "shake":
                        EnvTweaksPatches.Settings.AllowCameraShake = state;
                        break;
                }
                settingsFile.Save(EnvTweaksPatches.Settings);
            });
        }

        public override void Dispose()
        {
            _harmonyInstance.UnpatchAll(PatchCode);
        }
    }
}