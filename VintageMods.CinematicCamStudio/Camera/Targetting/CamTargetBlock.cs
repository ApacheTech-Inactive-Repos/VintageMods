using Vintagestory.API.MathTools;
using Vintagestory.API.Client;

namespace VintageMods.CinematicCamStudio.Camera.Targetting
{
    public class CamTargetBlock : CamTarget
    {
        private readonly BlockPos _blockPos;

        public CamTargetBlock(BlockPos blockPos)
        {
            _blockPos = blockPos;
        }

        public override EnumCamTargetType TargetType { get; } = EnumCamTargetType.Block;

        public override Vec3d GetPosition(IClientWorldAccessor world, float dt)
        {
            return GetPosition();
        }

        public override Vec3d GetPosition()
        {
            return new Vec3d(_blockPos.X + 0.5, _blockPos.Y + 0.5, _blockPos.Z + 0.5);
        }
    }
}