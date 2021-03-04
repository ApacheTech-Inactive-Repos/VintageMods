using System;
using System.IO;
using JetBrains.Annotations;
using VintageMods.Core.Extensions;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VintageMods.Core.IO
{
    /// <summary>
    ///     Provides a means for handling files, including embedded resources, used within a mod.
    /// </summary>
    public class FileManager
    {
        private readonly ICoreAPI _api;
        private readonly string _modFolder;

        /// <summary>
        ///     Initialises a new instance of the <see cref="FileManager" /> class.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="modFolder">Name of the mod.</param>
        public FileManager(ICoreAPI api, string modFolder)
        {
            _api = api;
            _modFolder = modFolder;
        }

        /// <summary>
        ///     Registers a new file with the file manager.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileType">Type of the file [Config | Data].</param>
        /// <param name="fileScope">The file scope [Global | World].</param>
        private ModFileInfo<T> RegisterFile<T>(string fileName, FileType fileType, FileScope fileScope)
            where T : class, new()
        {
            var seed = fileScope == FileScope.Global ? "" : _api.GetSeed();
            var file = new FileInfo(Path.Combine(GamePaths.DataPath, fileType.ToString(), _modFolder,
                fileScope.ToString(), seed, fileName));
            return new ModFileInfo<T>(file);
        }

        /// <summary>
        ///     Registers a new config file with the file manager.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileScope">The file scope [Global | World].</param>
        public ModFileInfo<T> RegisterConfigFile<T>(string fileName, FileScope fileScope) where T : class, new()
        {
            return RegisterFile<T>(fileName, FileType.Config, fileScope);
        }

        /// <summary>
        ///     Registers a new data file with the file manager.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileScope">The file scope [Global | World].</param>
        public ModFileInfo<T> RegisterDataFile<T>(string fileName, FileScope fileScope) where T : class, new()
        {
            return RegisterFile<T>(fileName, FileType.Data, fileScope);
        }

        /// <summary>
        ///     Purges all files and folders used by the mod.
        /// </summary>
        public void Purge()
        {
            try
            {
                ClearFolder(new DirectoryInfo(Path.Combine(GamePaths.DataPath, FileType.Config, _modFolder)));
                ClearFolder(new DirectoryInfo(Path.Combine(GamePaths.DataPath, FileType.Data, _modFolder)));
            }
            catch (Exception e)
            {
                _api.Logger.Error(e.StackTrace);
            }
        }

        /// <summary>
        ///     Searches for a file within the currently selected directory, and all child directories.
        /// </summary>
        /// <param name="dir">The root directory to perform the search from.</param>
        /// <param name="fileName">The name of the file to search for, including file extension.</param>
        /// <returns>Returns a FileInfo instance pertaining to the file, if it is found, otherwise returns null.</returns>
        [CanBeNull]
        public static FileInfo RecursiveSearch(DirectoryInfo dir, string fileName)
        {
            foreach (var fi in dir.GetFiles())
            {
                if (fi.Name.Equals(fileName))
                {
                    return fi;
                }
            }
            foreach (var di in dir.GetDirectories()) RecursiveSearch(di, fileName);
            return null;
        }

        private static void ClearFolder(DirectoryInfo dir)
        {
            foreach (var fi in dir.GetFiles()) fi.Delete();
            foreach (var di in dir.GetDirectories()) ClearFolder(di);
            dir.Delete();
        }
    }
}