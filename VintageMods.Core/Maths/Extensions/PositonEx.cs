using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace VintageMods.Core.Maths.Extensions
{
    /// <summary>
    ///     Extension Methods for the BlockPos class.
    /// </summary>
    public static class PositonEx
    {
        /// <summary>
        ///     Gets the position relative to spawn, given an absolute position within the game world.
        /// </summary>
        /// <param name="pos">The absolute position of the block being queried.</param>
        /// <param name="api">The API to use for game world information.</param>
        public static BlockPos RelativeToSpawn(this BlockPos pos, ICoreClientAPI api)
        {
            var blockPos = pos.SubCopy(api.World.DefaultSpawnPosition.XYZ.AsBlockPos);
            return new BlockPos(blockPos.X, pos.Y, blockPos.Z);
        }
    }
}