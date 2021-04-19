using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Targetting
{
    public class CamTargetSelf : CamTarget
    {
        private EntityPlayer _player;

        public override EnumCamTargetType TargetType { get; } = EnumCamTargetType.Self;

        public override Vec3d GetPosition(IClientWorldAccessor world, float dt)
        {
            if (_player == null)
            {
                _player = world.Player.Entity;
            }
            return GetPosition();
        }

        public override Vec3d GetPosition()
        {
            return new Vec3d(_player.Pos.X, _player.LocalEyePos.Y, _player.Pos.Z);
        }

        public CamTargetSelf(IClientWorldAccessor world)
        {
            _player = world.Player.Entity;
        }
    }
}