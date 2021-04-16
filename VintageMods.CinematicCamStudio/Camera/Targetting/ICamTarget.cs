using Vintagestory.API.MathTools;
using Vintagestory.API.Client;

namespace VintageMods.CinematicCamStudio.Camera.Targetting
{
    public interface ICamTarget
    {
        EnumCamTargetType TargetType { get; }

        Vec3d GetPosition(IClientWorldAccessor world, float dt);
    }
}