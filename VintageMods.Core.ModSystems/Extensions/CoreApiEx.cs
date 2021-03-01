using Vintagestory.API.Common;

namespace VintageMods.Core.ModSystems.Extensions
{
    /// <summary>
    ///     Extension Methods for the Core API.
    /// </summary>
    public static class CoreApiEx
    {
        public static string GetSeed(this ICoreAPI api)
        {
            return api?.World?.Seed.ToString();
        }
    }
}