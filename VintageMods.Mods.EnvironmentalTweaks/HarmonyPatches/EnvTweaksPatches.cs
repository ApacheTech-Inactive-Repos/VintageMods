using System;
using System.Linq;
using HarmonyLib;
using VintageMods.Core.Common.Reflection;
using VintageMods.Core.FileIO.Extensions;
using VintageMods.Mods.EnvironmentalTweaks.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
// ReSharper disable IdentifierTypo

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace VintageMods.Mods.EnvironmentalTweaks.HarmonyPatches
{
    [HarmonyPatch]
    public static class EnvTweaksPatches
    {
        internal static ICoreClientAPI Api { get; set; }
        internal static ModSettings Settings { get; set; }

        static EnvTweaksPatches()
        {
            var settingsFile = Api?.GetModFile("EnvTweaks.config.json");
            Settings = settingsFile?.ParseJsonAsObject<ModSettings>() ?? new ModSettings();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientMain), "AddCameraShake")]
        private static bool Patch_ClientMain_AddCameraShake()
        {
            return Settings.AllowCameraShake;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientMain), "SetCameraShake")]
        private static bool Patch_ClientMain_SetCameraShake()
        {
            return Settings.AllowCameraShake;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SystemRenderPlayerEffects), "onBeforeRender")]
        private static bool Patch_SystemRenderPlayerEffects_onBeforeRender(
            ref SystemRenderPlayerEffects __instance, float dt, ref ClientMain ___game, ref int ___maxDynLights, 
            ref NormalizedSimplexNoise ___noisegen, ref float ___curFreezingVal, ref long ___damangeVignettingUntil, 
            ref int ___duration, float ___strength)
        {
            var shUniforms = ___game.GetField<DefaultShaderUniforms>("shUniforms");
            var mouseYaw = ___game.GetField<float>("mouseYaw");
            var mousePitch = ___game.GetField<float>("mousePitch");

            shUniforms.PointLightsCount = 0;
            var plrPos = ___game.EntityPlayer.Pos.XYZ;
            var array = ___game.GetEntitiesAround(plrPos, 60f, 60f, e => e.LightHsv != null && e.LightHsv[2] > 0);
            if (array.Length > ___maxDynLights)
            {
                array = (from e in array
                         orderby e.Pos.SquareDistanceTo(plrPos)
                         select e).ToArray();
            }
            foreach (var entity in array)
            {
                var lightHsv = entity.LightHsv;
                __instance.CallMethod("AddPointLight", lightHsv, entity.Pos);
            }

            if (Api.IsGamePaused) return false;
            var treeAttribute = ___game.EntityPlayer.WatchedAttributes.GetTreeAttribute("health");
            var num = treeAttribute.GetFloat("currenthealth") / treeAttribute.GetFloat("maxhealth");
            var num2 = Math.Max(0f, (0.23f - num) * 1f / 0.18f);
            var num3 = 0f;
            if (num2 > 0f)
            {
                var num4 = (float)(___game.InWorldEllapsedMs / 1000.0);
                var num5 = (float)___noisegen.Noise(12412.0, num4 / 2f) * 0.5f + (float)Math.Pow(Math.Abs(GameMath.Sin(num4 * 1f / 0.7f)), 30.0) * 0.5f;
                num3 = Math.Min(num2 * 1.5f, 1f) * (num5 * 0.75f + 0.5f);
                if (___game.EntityPlayer.Alive && Settings.AllowCameraShake)
                {
                    shUniforms.ExtraSepia = GameMath.Clamp(num2 * (float)___noisegen.Noise(0.0, num4 / 3f) * 1.2f, 0f, 1.2f);
                    if (___game.Rand.NextDouble() < 0.01)
                    {
                        ___game.AddCameraShake(0.15f * num2);
                    }
                    ___game.SetField("mouseYaw", mouseYaw + num2 * (float)(___noisegen.Noise(76.0, num4 / 50f) - 0.5) * 0.003f);
                    var num6 = num2 * (float)(___noisegen.Noise(num4 / 50f, 987.0) - 0.5) * 0.003f;
                    ___game.EntityPlayer.Pos.Pitch += num6;
                    ___game.SetField("mousePitch", mousePitch + num6);
                }
            }
            else
            {
                shUniforms.ExtraSepia = 0f;
            }
            var num7 = GameMath.Clamp((int)(___damangeVignettingUntil - ___game.ElapsedMilliseconds), 0, ___duration);
            shUniforms.DamageVignetting = GameMath.Clamp(GameMath.Clamp(___strength / 2f, 0.5f, 3.5f) * (num7 / (float)Math.Max(1, ___duration)) + num3, 0f, 1.5f);
            var @float = ___game.EntityPlayer.WatchedAttributes.GetFloat("freezingEffectStrength");
            ___curFreezingVal += (@float - ___curFreezingVal) * dt;
            if (___curFreezingVal > 0.1 && Api.World.Player.CameraMode == EnumCameraMode.FirstPerson && Settings.AllowCameraShake)
            {
                var num8 = (float)(___game.InWorldEllapsedMs / 1000.0);
                ___game.SetField("mouseYaw", mouseYaw + ClientSettings.CameraShakeStrength * (float)(Math.Max(0.0, ___noisegen.Noise(num8, 12.0) - 0.40000000596046448) * Math.Sin(num8 * 90f) * 0.01) * GameMath.Clamp(___curFreezingVal * 3f, 0f, 1f));
            }
            shUniforms.FrostVignetting = ___curFreezingVal;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationLightning), "ClientTick")]
        private static bool Patch_WeatherSimulationLightning_ClientTick()
        {
            return Settings.AllowLightning;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationLightning), "OnRenderFrame")]
        private static bool Patch_WeatherSimulationLightning_OnRenderFrame()
        {
            return Settings.AllowLightning;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationSound), "updateSounds")]
        private static bool Patch_WeatherSimulationSound_updateSounds()
        {
            return Settings.AllowWeatherSounds;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "Initialize")]
        private static bool Prefix(ref WeatherSimulationParticles __instance)
        {
            _ws = __instance.GetField<WeatherSystemClient>("ws");
            _tmpPos = __instance.GetField<BlockPos>("tmpPos");
            _centerPos = __instance.GetField<BlockPos>("centerPos");
            _particlePos = __instance.GetField<Vec3d>("particlePos");
            _lowResRainHeightMap = __instance.GetField<int[,]>("lowResRainHeightMap");
            _parentVeloSnow = __instance.GetField<Vec3f>("parentVeloSnow");
            _rainParticleColor = __instance.GetField<int>("rainParticleColor");

            _splashParticles = __instance.GetField<SimpleParticleProperties>("splashParticles");
            _dustParticles = __instance.GetField<SimpleParticleProperties>("dustParticles");
            _rainParticle = __instance.GetField<SimpleParticleProperties>("rainParticle");
            _hailParticle = __instance.GetField<SimpleParticleProperties>("hailParticle");
            _snowParticle = __instance.GetField<SimpleParticleProperties>("snowParticle");

            _lblock = Api.World.GetBlock(new AssetLocation("water-still-7"));
            _rand = new Random(Api.World.Seed + 223123123);

            Api.Event.RegisterAsyncParticleSpawner(SpawnParticleAsync);
            return false;
        }

        private static bool SpawnParticleAsync(float dt, IAsyncParticleManager manager)
        {
            var weatherData = _ws.BlendedWeatherData;

            var conds = _ws.clientClimateCond;
            if (conds == null || !_ws.playerChunkLoaded) return true;

            EntityPos plrPos = Api.World.Player.Entity.Pos;
            var precIntensity = conds.Rainfall;

            var plevel = precIntensity * Api.Settings.Int["particleLevel"] / 100f;

            _tmpPos.Set((int)plrPos.X, (int)plrPos.Y, (int)plrPos.Z);

            var precType = weatherData.BlendedPrecType;
            if (precType == EnumPrecipitationType.Auto)
                precType = conds.Temperature < weatherData.snowThresholdTemp
                    ? EnumPrecipitationType.Snow
                    : EnumPrecipitationType.Rain;

            _particlePos.Set(Api.World.Player.Entity.Pos.X, Api.World.Player.Entity.Pos.Y,
                Api.World.Player.Entity.Pos.Z);

            var onwaterSplashParticleColor = Api.World.ApplyColorMapOnRgba(_lblock.ClimateColorMapForMap,
                _lblock.SeasonColorMapForMap, ColorUtil.WhiteArgb, (int)_particlePos.X, (int)_particlePos.Y,
                (int)_particlePos.Z, false);
            var col = ColorUtil.ToBGRABytes(onwaterSplashParticleColor);
            onwaterSplashParticleColor = ColorUtil.ToRgba(94, col[0], col[1], col[2]);

            _centerPos.Set((int)_particlePos.X, 0, (int)_particlePos.Z);
            for (var lx = 0; lx < 16; lx++)
            {
                var dx = (lx - 8) * 4;
                for (var lz = 0; lz < 16; lz++)
                {
                    var dz = (lz - 8) * 4;

                    _lowResRainHeightMap[lx, lz] =
                        Api.World.BlockAccessor.GetRainMapHeightAt(_centerPos.X + dx, _centerPos.Z + dz);
                }
            }

            var rainYPos = Api.World.BlockAccessor.GetRainMapHeightAt((int)_particlePos.X, (int)_particlePos.Z);

            _parentVeloSnow.X = -Math.Max(0, weatherData.curWindSpeed.X / 2 - 0.15f);
            _parentVeloSnow.Y = 0;
            _parentVeloSnow.Z = 0;

            if (weatherData.curWindSpeed.X > 0.7f && _particlePos.Y - rainYPos < 10)
            {
                var dx = (float)(plrPos.Motion.X * 40) - 50 * weatherData.curWindSpeed.X;
                var dy = (float)(plrPos.Motion.Y * 40);
                var dz = (float)(plrPos.Motion.Z * 40);

                _dustParticles.MinPos.Set(_particlePos.X - 40 + dx, _particlePos.Y + 15 + dy, _particlePos.Z - 40 + dz);
                _dustParticles.AddPos.Set(80, -20, 80);
                _dustParticles.GravityEffect = -0.1f - (float)_rand.NextDouble() * 0.1f;
                _dustParticles.ParticleModel = EnumParticleModel.Quad;
                _dustParticles.LifeLength = 1f;
                _dustParticles.DieOnRainHeightmap = true;
                _dustParticles.WindAffectednes = 8f;
                _dustParticles.MinQuantity = 0;
                _dustParticles.AddQuantity = 6 * (weatherData.curWindSpeed.X - 0.7f);

                _dustParticles.MinSize = 0.1f;
                _dustParticles.MaxSize = 0.4f;

                _dustParticles.MinVelocity.Set(-0.025f + 8 * weatherData.curWindSpeed.X, -0.2f, -0.025f);
                _dustParticles.AddVelocity.Set(0.05f + 4 * weatherData.curWindSpeed.X, 0.05f, 0.05f);


                for (var i = 0; i < 6; i++)
                {
                    var px = _particlePos.X + dx +
                             _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));
                    var pz = _particlePos.Z + dz +
                             _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));

                    var py = Api.World.BlockAccessor.GetRainMapHeightAt((int)px, (int)pz);
                    var block = Api.World.BlockAccessor.GetBlock((int)px, py, (int)pz);
                    if (block.IsLiquid()) continue;

                    _tmpPos.Set((int)px, py, (int)pz);
                    _dustParticles.Color = ColorUtil.ReverseColorBytes(block.GetColor(Api, _tmpPos));
                    _dustParticles.Color |= 255 << 24;

                    manager.Spawn(_dustParticles);
                }
            }

            if (precIntensity <= 0.02) return true;

            if (Settings.AllowHail && precType == EnumPrecipitationType.Hail)
            {
                var dx = (float)(plrPos.Motion.X * 40) - 4 * weatherData.curWindSpeed.X;
                var dy = (float)(plrPos.Motion.Y * 40);
                var dz = (float)(plrPos.Motion.Z * 40);

                _hailParticle.MinPos.Set(_particlePos.X + dx, _particlePos.Y + 15 + dy, _particlePos.Z + dz);

                _hailParticle.MinSize = 0.3f * (0.5f + conds.Rainfall);
                _hailParticle.MaxSize = 1f * (0.5f + conds.Rainfall);

                _hailParticle.Color = ColorUtil.ToRgba(220, 210, 230, 255);

                _hailParticle.MinQuantity = 100 * plevel;
                _hailParticle.AddQuantity = 25 * plevel;
                _hailParticle.MinVelocity.Set(-0.025f + 7.5f * weatherData.curWindSpeed.X, -5f, -0.025f);
                _hailParticle.AddVelocity.Set(0.05f + 7.5f * weatherData.curWindSpeed.X, 0.05f, 0.05f);

                manager.Spawn(_hailParticle);
                return true;
            }

            if (Settings.AllowRain && precType == EnumPrecipitationType.Rain)
            {
                var dx = (float)(plrPos.Motion.X * 80);
                var dy = (float)(plrPos.Motion.Y * 80);
                var dz = (float)(plrPos.Motion.Z * 80);

                _rainParticle.MinPos.Set(_particlePos.X - 30 + dx, _particlePos.Y + 15 + dy, _particlePos.Z - 30 + dz);
                _rainParticle.WithTerrainCollision = false;
                _rainParticle.MinQuantity = 1000 * plevel;
                _rainParticle.LifeLength = 1f;
                _rainParticle.AddQuantity = 25 * plevel;
                _rainParticle.MinSize = 0.15f * (0.5f + conds.Rainfall);
                _rainParticle.MaxSize = 0.22f * (0.5f + conds.Rainfall);
                _rainParticle.Color = _rainParticleColor;

                _rainParticle.MinVelocity.Set(-0.025f + 8 * weatherData.curWindSpeed.X, -10f, -0.025f);
                _rainParticle.AddVelocity.Set(0.05f + 8 * weatherData.curWindSpeed.X, 0.05f, 0.05f);

                manager.Spawn(_rainParticle);

                _splashParticles.MinVelocity = new Vec3f(-1f, 3, -1f);
                _splashParticles.AddVelocity = new Vec3f(2, 0, 2);
                _splashParticles.LifeLength = 0.1f;
                _splashParticles.MinSize = 0.07f * (0.5f + 0.65f * conds.Rainfall);
                _splashParticles.MaxSize = 0.2f * (0.5f + 0.65f * conds.Rainfall);
                _splashParticles.ShouldSwimOnLiquid = true;
                _splashParticles.Color = _rainParticleColor;

                var cnt = 100 * plevel;

                for (var i = 0; i < cnt; i++)
                {
                    var px = _particlePos.X + _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));
                    var pz = _particlePos.Z + _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));

                    var py = Api.World.BlockAccessor.GetRainMapHeightAt((int)px, (int)pz);

                    var block = Api.World.BlockAccessor.GetBlock((int)px, py, (int)pz);

                    if (block.IsLiquid())
                    {
                        _splashParticles.MinPos.Set(px, py + block.TopMiddlePos.Y - 1 / 8f, pz);
                        _splashParticles.AddVelocity.Y = 1.5f;
                        _splashParticles.LifeLength = 0.17f;
                        _splashParticles.Color = onwaterSplashParticleColor;
                    }
                    else
                    {
                        var b = 0.75 + 0.25 * _rand.NextDouble();
                        var ca = 230 - _rand.Next(100);
                        var cr = (int)(((_rainParticleColor >> 16) & 0xff) * b);
                        var cg = (int)(((_rainParticleColor >> 8) & 0xff) * b);
                        var cb = (int)(((_rainParticleColor >> 0) & 0xff) * b);

                        _splashParticles.Color = (ca << 24) | (cr << 16) | (cg << 8) | cb;
                        _splashParticles.AddVelocity.Y = 0f;
                        _splashParticles.LifeLength = 0.1f;
                        _splashParticles.MinPos.Set(px, py + block.TopMiddlePos.Y + 0.05, pz);
                    }

                    manager.Spawn(_splashParticles);
                }
            }

            if (Settings.AllowSnow && precType == EnumPrecipitationType.Snow)
            {
                var wetness = 2.5f * GameMath.Clamp(_ws.clientClimateCond.Temperature + 1, 0, 4) / 4f;

                var dx = (float)(plrPos.Motion.X * 40) - (30 - 9 * wetness) * weatherData.curWindSpeed.X;
                var dy = (float)(plrPos.Motion.Y * 40);
                var dz = (float)(plrPos.Motion.Z * 40);

                _snowParticle.MinVelocity.Set(-0.5f + 5 * weatherData.curWindSpeed.X, -1f, -0.5f);
                _snowParticle.AddVelocity.Set(1f + 5 * weatherData.curWindSpeed.X, 0.05f, 1f);
                _snowParticle.Color = ColorUtil.ToRgba(255, 255, 255, 255);

                _snowParticle.MinQuantity = 90 * plevel * (1 + wetness / 3);
                _snowParticle.AddQuantity = 15 * plevel * (1 + wetness / 3);
                _snowParticle.ParentVelocity = _parentVeloSnow;
                _snowParticle.ShouldDieInLiquid = true;

                _snowParticle.LifeLength = Math.Max(1f, 4f - wetness);
                _snowParticle.Color = ColorUtil.ColorOverlay(ColorUtil.ToRgba(255, 255, 255, 255), _rainParticle.Color,
                    wetness / 4f);
                _snowParticle.GravityEffect = 0.005f * (1 + 20 * wetness);
                _snowParticle.MinSize = 0.1f * conds.Rainfall;
                _snowParticle.MaxSize = 0.3f * conds.Rainfall / (1 + wetness);

                const float hrange = 40f;
                const float vrange = 20f;
                _snowParticle.MinPos.Set(_particlePos.X - hrange + dx, _particlePos.Y + vrange + dy,
                    _particlePos.Z - hrange + dz);
                _snowParticle.AddPos.Set(2 * hrange + dx, -0.66f * vrange + dy, 2 * hrange + dz);

                manager.Spawn(_snowParticle);
            }

            return true;
        }

        private static Random _rand;

        private static WeatherSystemClient _ws;
        private static BlockPos _tmpPos = new BlockPos();
        private static BlockPos _centerPos = new BlockPos();
        private static Vec3d _particlePos = new Vec3d();
        private static Block _lblock;
        private static int[,] _lowResRainHeightMap = new int[16, 16];
        private static Vec3f _parentVeloSnow = new Vec3f();
        private static int _rainParticleColor;

        private static SimpleParticleProperties _splashParticles;
        private static SimpleParticleProperties _dustParticles;
        private static SimpleParticleProperties _rainParticle;
        private static SimpleParticleProperties _hailParticle;
        private static SimpleParticleProperties _snowParticle;
    }
}