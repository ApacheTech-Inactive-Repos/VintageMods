using System;
using System.Linq;
using System.Reflection;
using VintageMods.Core.Attributes;
using Vintagestory.API.Common;

namespace VintageMods.Core.IO.Extensions
{
    public static class ApiExtensions
    {
        private static FileManager _fileManagerInstance;

        /// <summary>
        ///     Registers a file manager with the mod.
        /// </summary>
        /// <param name="api">The core game API.</param>
        /// <returns>A file manager that can be used to read from, and write to the filesystem.</returns>
        public static FileManager RegisterFileManager(this ICoreAPI api)
        {
            var rootFolder = Assembly.GetExecutingAssembly().GetCustomAttributes()
                .OfType<ModDomainAttribute>().FirstOrDefault()?.RootFolder ?? "Common";
            _fileManagerInstance = new FileManager(api, rootFolder);
            return _fileManagerInstance;
        }

        /// <summary>
        ///     Retrieves a registered file from the mod's file manager.
        /// </summary>
        /// <param name="api">The core game API.</param>
        /// <param name="fileName">The name of the file to retrieve.</param>
        /// <returns>A previously registered file that can be read from, or written to.</returns>
        public static ModFileInfo GetModFile(this ICoreAPI api, string fileName)
        {
            try
            {
                return _fileManagerInstance[fileName];
            }
            catch (Exception e)
            {
                api.Logger.Audit("[VintageMods] Error occurred locating file. Check Error Log.");
                api.Logger.Error($"[VintageMods] {e.Message}");
                throw;
            }
        }
    }
}