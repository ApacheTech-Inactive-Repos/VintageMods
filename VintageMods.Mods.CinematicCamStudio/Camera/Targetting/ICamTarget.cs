using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Targetting
{
    public interface ICamTarget
    {
        EnumCamTargetType TargetType { get; }

        Vec3d GetPosition(IClientWorldAccessor world, float dt);
    }
}