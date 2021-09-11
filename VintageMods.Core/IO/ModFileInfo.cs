using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VintageMods.Core.IO.Extensions;
using Vintagestory.API.Datastructures;

namespace VintageMods.Core.IO
{
    /// <summary>
    ///     Manages a file used to collate and store mod information. This class cannot be inherited.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ModFileInfo
    {
        private readonly FileInfo _fileOnDisk;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFileInfo" /> class.
        /// </summary>
        /// <param name="filePath">The path to the file on disk to use for IO operations.</param>
        public ModFileInfo(string filePath)
        {
            _fileOnDisk = new FileInfo(filePath);
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
            if (!_fileOnDisk.Exists && ResourceManager.ResourceExists(typeof(TModel).Assembly, _fileOnDisk.Name))
                DisembedFrom(typeof(TModel).Assembly);
            return JsonConvert.DeserializeObject<List<TModel>>(File.ReadAllText(_fileOnDisk.FullName));
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        public TModel ParseAsProtoObject<TModel>() where TModel : class, new()
        {
            var bytes = (_fileOnDisk.Exists) ? File.ReadAllBytes(_fileOnDisk.FullName) : new byte[]{ };
            return ProtoEx.Deserialise<TModel>(bytes);
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        /// </summary>
        public JsonObject AsRawJsonObject()
        {
            if (_fileOnDisk.Exists) return JsonObject.FromJson(_fileOnDisk.OpenText().ReadToEnd());
            if (ResourceManager.ResourceExists(Assembly.GetCallingAssembly(), _fileOnDisk.Name))
                DisembedFrom(Assembly.GetCallingAssembly());
            else
                throw new FileNotFoundException($"Cannot find physical file, or embedded resource for file: { _fileOnDisk.Name }");
            return JsonObject.FromJson(_fileOnDisk.OpenText().ReadToEnd());
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed list.
        /// </summary>
        /// <typeparam name="TModel">The type of list to deserialise into.</typeparam>
        public List<TModel> ParseAsProtoList<TModel>() where TModel : class, new()
        {
            var bytes = (_fileOnDisk.Exists) ? File.ReadAllBytes(_fileOnDisk.FullName) : new byte[] { };
            return ProtoEx.Deserialise<List<TModel>>(bytes);
        }

        /// <summary>
        ///     Copies the contents of an embedded resource to disk.
        ///     The embedded resource is read from the same assembly as the underlying object model type,
        ///     and using the same filename as used to instantiate this class.
        /// </summary>
        public ModFileInfo DisembedFrom(Assembly assembly)
        {
            if (!ResourceManager.ResourceExists(assembly, _fileOnDisk.Name)) return this;
            using var stream = ResourceManager.GetResourceStream(assembly, _fileOnDisk.Name);
            using var file = File.OpenWrite(_fileOnDisk.FullName);
            stream.CopyTo(file);
            return this;
        }

        /// <summary>
        ///     Copies the contents of an embedded resource to disk.
        ///     The embedded resource is read from the same assembly as the underlying object model type,
        ///     and using the same filename as used to instantiate this class.
        /// </summary>
        public ModFileInfo Disembed()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (!ResourceManager.ResourceExists(assembly, _fileOnDisk.Name)) return this;
            using var stream = ResourceManager.GetResourceStream(assembly, _fileOnDisk.Name);
            using var file = File.OpenWrite(_fileOnDisk.FullName);
            stream.CopyTo(file);
            return this;
        }

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting JSON to file.
        /// </summary>
        public void SaveAsJson<TModel>(TModel instance) where TModel : class, new()
        {
            SaveJsonToDisk(JsonConvert.SerializeObject(instance, Formatting.Indented));
        }

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting ProtoContract to file.
        /// </summary>
        public void SaveAsProtoContract<TModel>(TModel instance) where TModel : class, new()
        {
            SaveBinaryToDisk(ProtoEx.Serialise(instance));
        }

        /// <summary>
        ///     Gets a value indicating whether a file exists.
        /// </summary>
        /// <returns>true if the file exists; false if the file does not exist or if the file is a directory.</returns>
        public bool Exists => _fileOnDisk.Exists;

        /// <summary>
        ///     Gets the full path of the file.
        /// </summary>
        /// <returns>A string containing the full path.</returns>
        public string Path => _fileOnDisk.FullName;

        private void SaveJsonToDisk(string contents)
        {
            Directory.CreateDirectory(_fileOnDisk.DirectoryName ?? string.Empty);
            File.WriteAllText(_fileOnDisk.FullName, contents);
        }

        private void SaveBinaryToDisk(IEnumerable<byte> contents)
        {
            Directory.CreateDirectory(_fileOnDisk.DirectoryName ?? string.Empty);
            File.WriteAllBytes(_fileOnDisk.FullName, contents.ToArray());
        }
    }
}