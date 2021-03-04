using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VintageMods.Core.IO
{
    /// <summary>
    ///     Manages a file used to collate and store mod information.
    /// </summary>
    /// <typeparam name="TModel">The strongly-typed object model, of which the data within the file represents.</typeparam>
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    public class ModFileInfo<TModel> where TModel : class, new()
    {
        private readonly FileInfo _fileOnDisk;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFileInfo{T}" /> class.
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
        public TModel ParseJsonAsObject()
        {
            if (!_fileOnDisk.Exists) Disembed();
            return JsonConvert.DeserializeObject<TModel>(File.ReadAllText(_fileOnDisk.FullName));
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed list.
        /// </summary>
        /// <typeparam name="TModel">The type of list to deserialise into.</typeparam>
        public List<TModel> ParseJsonAsList()
        {
            if (!_fileOnDisk.Exists) Disembed();
            return JsonConvert.DeserializeObject<List<TModel>>(File.ReadAllText(_fileOnDisk.FullName));
        }

        /// <summary>
        ///     Copies the contents of an embedded resource to disk.
        ///     The embedded resource is read from the same assembly as the underlying object model type,
        ///     and using the same filename as used to instantiate this class.
        /// </summary>
        public void Disembed()
        {
            SaveToDisk(ResourceManager.ReadResourceRaw(typeof(TModel).Assembly, _fileOnDisk.Name));
        }

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting JSON to file.
        /// </summary>
        public void Save(TModel instance)
        {
            SaveToDisk(JsonConvert.SerializeObject(instance, Formatting.Indented));
        }

        private void SaveToDisk(string contents)
        {
            Directory.CreateDirectory(_fileOnDisk.DirectoryName ?? string.Empty);
            File.WriteAllText(_fileOnDisk.FullName, contents);
        }
    }
}