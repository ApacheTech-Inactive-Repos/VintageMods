using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace VintageMods.Core.FileIO
{
    /// <summary>
    ///     Manages a file used to collate and store mod information.
    /// </summary>
    public class ModFileInfo
    {
        private readonly FileInfo _fileOnDisk;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFileInfo" /> class.
        /// </summary>
        /// <param name="fileInfo">The file on disk to use for IO operations.</param>
        public ModFileInfo(FileInfo fileInfo)
        {
            _fileOnDisk = fileInfo;
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        public TModel ParseJsonAsObject<TModel>() where TModel : class, new()
        {
            if (_fileOnDisk.Exists)
                return JsonConvert.DeserializeObject<TModel>(File.ReadAllText(_fileOnDisk.FullName));

            DisembedFrom(typeof(TModel).Assembly);
            return JsonConvert.DeserializeObject<TModel>(File.ReadAllText(_fileOnDisk.FullName));
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed list.
        /// </summary>
        /// <typeparam name="TModel">The type of list to deserialise into.</typeparam>
        public List<TModel> ParseJsonAsList<TModel>() where TModel : class, new()
        {
            if (_fileOnDisk.Exists)
                return JsonConvert.DeserializeObject<List<TModel>>(File.ReadAllText(_fileOnDisk.FullName));

            DisembedFrom(typeof(TModel).Assembly);
            return JsonConvert.DeserializeObject<List<TModel>>(File.ReadAllText(_fileOnDisk.FullName));
        }

        /// <summary>
        ///     Copies the contents of an embedded resource to disk.
        ///     The embedded resource is read from the same assembly as the underlying object model type,
        ///     and using the same filename as used to instantiate this class.
        /// </summary>
        public void DisembedFrom(Assembly assembly)
        {
            if (ResourceManager.ResourceExists(assembly, _fileOnDisk.Name))
                SaveToDisk(ResourceManager.ReadResourceRaw(assembly, _fileOnDisk.Name));
        }

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting JSON to file.
        /// </summary>
        public void Save<TModel>(TModel instance) where TModel : class, new()
        {
            SaveToDisk(JsonConvert.SerializeObject(instance, Formatting.Indented));
        }

        /// <summary>
        ///     Gets a value indicating whether a file exists.
        /// </summary>
        /// <returns>true if the file exists; false if the file does not exist or if the file is a directory.</returns>
        public bool Exists => _fileOnDisk.Exists;

        private void SaveToDisk(string contents)
        {
            Directory.CreateDirectory(_fileOnDisk.DirectoryName ?? string.Empty);
            File.WriteAllText(_fileOnDisk.FullName, contents);
        }
    }
}