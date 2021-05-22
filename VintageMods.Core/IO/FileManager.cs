using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using VintageMods.Core.IO.Enum;
using VintageMods.Core.IO.Extensions;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VintageMods.Core.IO
{
    /// <summary>
    ///     Provides a means for handling files, including embedded resources, used within a mod.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class FileManager
    {
        private static ICoreAPI _api;

        /// <summary>
        ///     Initialises static members of the <see cref="FileManager" /> class.
        /// </summary>
        static FileManager()
        {
            VintageModsRootPath = CreateDirectory(Path.Combine(GamePaths.DataPath, "ModData", "VintageMods"));
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="FileManager" /> class.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="modFolderName">Name of the mod.</param>
        public FileManager(ICoreAPI api, string modFolderName)
        {
            _api = api;
            ModFiles = new Dictionary<string, ModFileInfo>();
            ModRootPath = CreateDirectory(Path.Combine(VintageModsRootPath, modFolderName));
            ModGlobalPath = CreateDirectory(Path.Combine(ModRootPath, "Global"));
            ModWorldPath = CreateDirectory(Path.Combine(ModRootPath, "World", api.World.SavegameIdentifier));
        }

        /// <summary>
        ///     Gets the root path for all VintageMods mod files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string VintageModsRootPath { get; }

        /// <summary>
        ///     Gets the path used for storing data files for a particular mod.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public string ModRootPath { get; }

        /// <summary>
        ///     Gets the path used for storing global data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public string ModGlobalPath { get; }

        /// <summary>
        ///     Gets the path used for storing per-world data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public string ModWorldPath { get; }

        private Dictionary<string, ModFileInfo> ModFiles { get; }

        /// <summary>
        ///     Gets the <see cref="ModFileInfo" /> with the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public ModFileInfo this[string fileName]
        {
            get
            {
                try
                {
                    return ModFiles[fileName];
                }
                catch (Exception e)
                {
                    _api.Logger.Audit("[VintageMods] Error occurred locating file. Check Error Log.");
                    _api.Logger.Error($"[VintageMods] {e.Message}");
                    throw;
                }
            }
        }

        private static string CreateDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists) dir.Create();
            _api?.Logger.VerboseDebug($"[VintageMods] Creating folder: {dir}");
            return dir.FullName;
        }

        /// <summary>
        ///     Registers a new file with the file manager.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileScope">The file scope [Global | World].</param>
        public ModFileInfo RegisterFile(string fileName, FileScope fileScope)
        {
            var modFile = new ModFileInfo(GetScopedPath(fileScope, fileName));
            ModFiles.Add(fileName, modFile);
            if (!modFile.Exists) modFile.DisembedFrom(Assembly.GetCallingAssembly());
            return modFile;
        }

        /// <summary>
        ///     Purges all files and folders used by the mod.
        /// </summary>
        public void Purge()
        {
            try
            {
                new DirectoryInfo(ModRootPath).RecursivePurge();
            }
            catch (Exception e)
            {
                _api.Logger.VerboseDebug("[VintageMods] Error occurred. Check Error Log.");
                _api.Logger.Error(e.StackTrace);
            }
        }

        private string GetScopedPath(FileScope scope, string fileName)
        {
            return Path.Combine(scope == FileScope.World ? ModWorldPath : ModGlobalPath, fileName);
        }
    }
}