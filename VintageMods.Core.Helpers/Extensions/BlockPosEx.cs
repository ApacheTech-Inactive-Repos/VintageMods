using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace VintageMods.Core.Helpers.Extensions
{
    /// <summary>
    ///     Extension Methods for the BlockPos class.
    /// </summary>
    public static class BlockPosEx
    {
        /// <summary>
        ///     Gets the position relative to spawn, given an absolute position within the game world.
        /// </summary>
        /// <param name="pos">The absolute position of the block being queried.</param>
        /// <param name="api">The API to use for game world information.</param>
        /// <param name="relY">if set to <c>true</c>, the Y axis is also converted, otherwise, only the X and Z axis values are changed. Default: false.</param>
        /// <returns>BlockPos.</returns>
        public static BlockPos RelativeToSpawn(this BlockPos pos, ICoreClientAPI api, bool relY = false)
        {
            var blockPos = pos.SubCopy(api.World.DefaultSpawnPosition.XYZ.AsBlockPos);
            return new BlockPos(blockPos.X, relY ? blockPos.Y : pos.Y, blockPos.Z);
        }
    }
}