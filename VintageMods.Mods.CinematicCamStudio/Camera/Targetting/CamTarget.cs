using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Targetting
{
    public abstract class CamTarget : ICamTarget
    {
        public abstract EnumCamTargetType TargetType { get; }
        public abstract Vec3d GetPosition(IClientWorldAccessor world, float dt);
        public abstract Vec3d GetPosition();
    }
}