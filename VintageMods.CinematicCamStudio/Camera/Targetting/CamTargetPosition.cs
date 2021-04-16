using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace VintageMods.CinematicCamStudio.Camera.Targetting
{
    public class CamTargetPosition : CamTarget
    {
        private readonly Vec3d _position;

        public CamTargetPosition(Vec3d pos)
        {
            _position = pos;
        }

        public CamTargetPosition(double x, double y, double z) : this(new Vec3d(x, y, z)) { }

        public CamTargetPosition(EntityPos pos) : this(new Vec3d(pos.X, pos.Y, pos.Z)) { }

        public override EnumCamTargetType TargetType { get; } = EnumCamTargetType.Position;

        public override Vec3d GetPosition(IClientWorldAccessor world, float dt)
        {

            return GetPosition();
        }

        public override Vec3d GetPosition()
        {
            return _position;
        }
    }
}