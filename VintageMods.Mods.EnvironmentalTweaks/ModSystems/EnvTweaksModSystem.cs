using System.Text;
using VintageMods.Core.Attributes;
using VintageMods.Core.IO.Enum;
using VintageMods.Core.IO.Extensions;
using VintageMods.Core.ModSystems;
using VintageMods.Mods.EnvironmentalTweaks.Config;
using VintageMods.Mods.EnvironmentalTweaks.HarmonyPatches;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable UnusedType.Global

[assembly: ModDomain("envtweaks", "EnvTweaks")]

namespace VintageMods.Mods.EnvironmentalTweaks.ModSystems
{
    internal class EnvTweaksModSystem : ClientSideModSystem
    {
        public override void StartClientSide(ICoreClientAPI api)
        {
            var settingsFile = api.RegisterFileManager().RegisterFile("EnvTweaks.config.json", FileScope.Global);
            EnvTweaksPatches.Api = api;
            EnvTweaksPatches.Settings = settingsFile.ParseJsonAsObject<ModSettings>();

            void Handler(int _, CmdArgs args)
            {
                if (args.Length == 0)
                {
                    api.ShowChatMessage("Environmental Tweaks: [lightning|rain|hail|snow|sounds|shake] [on|off]");
                    return;
                }

                var option = args.PopWord();
                var state = args.PopWord("off").ToLowerInvariant() == "on";
                switch (option)
                {
                    case "settings":
                        var sb = new StringBuilder();
                        sb.AppendLine("Environmental Tweaks:\n");
                        sb.AppendLine($"Lightning Effects: {EnvTweaksPatches.Settings.AllowLightning}");
                        sb.AppendLine($"Weather Sounds: {EnvTweaksPatches.Settings.AllowWeatherSounds}");
                        sb.AppendLine($"Rainfall Particles: {EnvTweaksPatches.Settings.AllowRain}");
                        sb.AppendLine($"Hail Particles: {EnvTweaksPatches.Settings.AllowHail}");
                        sb.AppendLine($"Snow Particles: {EnvTweaksPatches.Settings.AllowSnow}");
                        sb.AppendLine($"Camera Shake: {EnvTweaksPatches.Settings.AllowCameraShake}");
                        api.SendChatMessage(".clearchat");
                        api.ShowChatMessage(sb.ToString());
                        break;
                    case "lightning":
                        api.ShowChatMessage($"Lightning Effects: {state}");
                        EnvTweaksPatches.Settings.AllowLightning = state;
                        break;

                    case "sounds":
                        api.ShowChatMessage($"Weather Sounds: {state}");
                        EnvTweaksPatches.Settings.AllowWeatherSounds = state;
                        break;

                    case "rain":
                        api.ShowChatMessage($"Rainfall Particles: {state}");
                        EnvTweaksPatches.Settings.AllowRain = state;
                        break;

                    case "hail":
                        api.ShowChatMessage($"Hail Particles: {state}");
                        EnvTweaksPatches.Settings.AllowHail = state;
                        break;

                    case "snow":
                        api.ShowChatMessage($"Snow Particles: {state}");
                        EnvTweaksPatches.Settings.AllowSnow = state;
                        break;

                    case "shake":
                        api.ShowChatMessage($"Camera Shake: {state}");
                        EnvTweaksPatches.Settings.AllowCameraShake = state;
                        break;

                    default:
                        api.ShowChatMessage("Environmental Tweaks: [settings|lightning|rain|hail|snow|sounds|shake] [on|off]");
                        break;
                }

                settingsFile.SaveAsJson(EnvTweaksPatches.Settings);
            }

            api.RegisterCommand("EnvTweaks", 
                "Change settings for Environmental Tweaks.",
                "[settings|lightning|rain|hail|snow|sounds|shake] [on|off]", Handler);

            api.RegisterCommand("et",
                "Change settings for Environmental Tweaks.",
                "[settings|lightning|rain|hail|snow|sounds|shake] [on|off]", Handler);
        }
    }
}