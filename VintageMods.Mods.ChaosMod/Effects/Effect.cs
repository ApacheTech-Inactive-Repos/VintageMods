using VintageMods.Mods.ChaosMod.Engine.Enums;

namespace VintageMods.Mods.ChaosMod.Effects
{
    internal abstract class EffectBase
    {
        internal abstract EffectDuration Duration { get; }

        internal abstract void OnStart();
        internal abstract void OnStop();
        internal abstract void OnTick();
    }
}