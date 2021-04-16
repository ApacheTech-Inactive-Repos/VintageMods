using VintageMods.CinematicCamStudio.Exceptions;
using Vintagestory.API.MathTools;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VintageMods.CinematicCamStudio.Camera.Targetting
{
    public class CamTargetPlayer : CamTarget
    {
        private EntityPlayer _player;
        private readonly string _playerName;

        public override EnumCamTargetType TargetType { get; } = EnumCamTargetType.Player;

        public CamTargetPlayer(string playerName)
        {
            _playerName = playerName;
        }

        public override Vec3d GetPosition(IClientWorldAccessor world, float dt)
        {
            if (!(_player is null))
            {
                return new Vec3d(_player.Pos.X, _player.LocalEyePos.Y, _player.Pos.Z);
            }
            
            foreach (var player in world.AllOnlinePlayers)
            {
                if (player.PlayerName == _playerName)
                {
                    _player = player.Entity;
                }
            }
            
            if (_player is null)
            {
                throw new CamStudioException("The specified target player is not currently online.");
            }


            return GetPosition();
        }

        public override Vec3d GetPosition()
        {
            return new Vec3d(_player.Pos.X, _player.LocalEyePos.Y, _player.Pos.Z);
        }

        public CamTargetPlayer(IClientWorldAccessor world)
        {
            _player = world.Player.Entity;
        }
    }
}