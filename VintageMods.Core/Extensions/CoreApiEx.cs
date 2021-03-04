using Vintagestory.API.Common;

namespace VintageMods.Core.Extensions
{
    /// <summary>
    ///     Extension Methods for the Core API.
    /// </summary>
    public static class CoreApiEx
    {
        /// <summary>
        ///     Gets the world seed.
        /// </summary>
        /// <param name="api">The core game API.</param>
        public static string GetSeed(this ICoreAPI api)
        {
            return api?.World?.Seed.ToString();
        }
    }
}