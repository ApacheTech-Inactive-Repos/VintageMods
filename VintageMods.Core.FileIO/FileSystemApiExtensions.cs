using Vintagestory.API.Common;

namespace VintageMods.Core.FileIO
{
    public static class FileSystemApiExtensions
    {
        private static FileManager _fileManagerInstance;

        public static void RegisterFileManager(this ICoreAPI api, string rootFolder)
        {
            _fileManagerInstance = new FileManager(api, rootFolder);
        }

        public static FileManager FileManager(this ICoreAPI api, string rootFolder = null)
        {
            if (string.IsNullOrEmpty(rootFolder)) return _fileManagerInstance;
            return _fileManagerInstance ??= new FileManager(api, rootFolder);
        }
    }
}