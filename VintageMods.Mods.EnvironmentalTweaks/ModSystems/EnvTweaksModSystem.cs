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
            var syntaxMessage = "[settings|lightning|rain|hail|snow|sounds|shake|fog|clouds] [on|off]";

            void Handler(int _, CmdArgs args)
            {
                if (args.Length == 0)
                {
                    api.ShowChatMessage($"Environmental Tweaks: {syntaxMessage}");
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
                        sb.AppendLine($"Fog Effects: {EnvTweaksPatches.Settings.AllowFog}");
                        sb.AppendLine($"Fog Effects: {EnvTweaksPatches.Settings.AllowFog}");
                        sb.AppendLine($"Show Clouds: {EnvTweaksPatches.Settings.AllowClouds}");
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

                    case "fog":
                        api.ShowChatMessage($"Fog Effects: {state}");
                        EnvTweaksPatches.Settings.AllowFog = state;
                        break;

                    case "clouds":
                        api.ShowChatMessage($"Show Clouds: {state}");
                        EnvTweaksPatches.Settings.AllowClouds = state;
                        break;

                    default:
                        api.ShowChatMessage($"Environmental Tweaks: {syntaxMessage}");
                        break;
                }

                settingsFile.SaveAsJson(EnvTweaksPatches.Settings);
            }

            api.RegisterCommand("EnvTweaks", "Change settings for Environmental Tweaks.", syntaxMessage, Handler);
            api.RegisterCommand("et", "Change settings for Environmental Tweaks.", syntaxMessage, Handler);
        }

        public EnvTweaksModSystem() : base("envtweaks")
        {
        }
    }
}