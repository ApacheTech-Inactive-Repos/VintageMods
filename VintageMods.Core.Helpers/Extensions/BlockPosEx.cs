using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace VintageMods.Core.Helpers.Extensions
{
    public static class BlockPosEx
    {
        public static BlockPos RelativeToSpawn(this BlockPos pos, ICoreClientAPI api, bool relY = false)
        {
            var blockPos = pos.SubCopy(api.World.DefaultSpawnPosition.XYZ.AsBlockPos);
            return new BlockPos(blockPos.X, relY ? blockPos.Y : pos.Y, blockPos.Z);
        }
    }
}