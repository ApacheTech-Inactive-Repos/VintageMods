using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VintageMods.Core.ModSystems.Extensions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VintageMods.Core.ModSystems.IO
{
    /// <summary>
    ///     Provides a means for handling files, including embedded resources, used within a mod.
    /// </summary>
    public class FileManager
    {
        private readonly string _modFolder;

        private readonly ICoreAPI _api;

        private readonly Dictionary<string, FileInfo> _modFiles = new Dictionary<string, FileInfo>();
        
        /// <summary>
        ///     Initialises a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="modFolder">Name of the mod.</param>
        public FileManager(ICoreAPI api, string modFolder)
        {
            _api = api;
            _modFolder = modFolder;
        }

        /// <summary>
        ///     Registers a new file with the mode file manager.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileType">Type of the file [Config | Data].</param>
        /// <param name="fileScope">The file scope [Global | World].</param>
        public void RegisterFile(string fileName, FileType fileType, FileScope fileScope)
        {
            var seed = fileScope == FileScope.Global ? "" : _api.GetSeed();
            _modFiles.Add(fileName,
                new FileInfo(Path.Combine(GamePaths.DataPath, fileType, _modFolder, fileScope, seed, fileName)));
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialise.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        public T As<T>(string fileName) where T : class, new()
        {
            return Deserialise<T>(Validate<T>(fileName));
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed list.
        /// </summary>
        /// <typeparam name="T">The type of list to deserialise.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        public List<T> AsListOf<T>(string fileName) where T : class, new()
        {
            return Deserialise<List<T>>(Validate<T>(fileName));
        }

        /// <summary>
        ///     Saves a file to disk, from an embedded resource within the assembly of the
        ///     strongly-typed generic constraint.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the file.</param>
        public void SaveFromResource<T>(string fileName) where T : class, new()
        {
            try
            {
                SaveFromResource<T>(Validate<T>(fileName));
            }
            catch (Exception ex)
            {
                _api.Logger.Error($"Failed saving file ({fileName}), error {ex.Message}.");
            }
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
                _modFiles.Clear();
            }
            catch (Exception e)
            {
                _api.Logger.Error(e.StackTrace);
            }
        }

        private FileInfo Validate<T>(string fileName) where T : class, new()
        {
            if (!_modFiles.ContainsKey(fileName))
                throw new FileNotFoundException("File has not yet been registered with the file manager.", fileName);
            var fileInfo = _modFiles[fileName];
            if (!fileInfo.Exists) SaveFromResource<T>(fileInfo);
            return fileInfo;
        }

        private static T Deserialise<T>(FileSystemInfo fileInfo)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileInfo.FullName));
        }

        private static void SaveFromResource<T>(FileInfo fileInfo) where T : class, new()
        {
            Directory.CreateDirectory(fileInfo.DirectoryName ?? string.Empty);
            var contents = ResourceManager.ReadResourceRaw(typeof(T).Assembly, fileInfo.Name);
            File.WriteAllText(fileInfo.FullName, contents);
        }

        private static void ClearFolder(DirectoryInfo dir)
        {
            foreach (var fi in dir.GetFiles()) fi.Delete();
            foreach (var di in dir.GetDirectories()) ClearFolder(di);
            dir.Delete();
        }
    }
}