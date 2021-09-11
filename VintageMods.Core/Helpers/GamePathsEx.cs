using System.IO;
using System.Reflection;

namespace VintageMods.Core.Helpers
{
    public static class GamePathsEx
    {
        public static string ModRootDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string AssetsDirectory => Path.Combine(ModRootDirectory, "assets");
    }
}