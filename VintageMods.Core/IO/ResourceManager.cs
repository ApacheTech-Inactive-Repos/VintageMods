﻿using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VintageMods.Core.Helpers;
using VintageMods.Core.IO.Extensions;
using Vintagestory.API.Util;

namespace VintageMods.Core.IO
{
    /// <summary>
    ///     Controls access to embedded resources within assemblies,
    ///     allowing them to be parsed as strings or strongly-typed objects.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ResourceManager
    {
        /// <summary>
        ///     Parses the resource into a strongly-typed object.
        /// </summary>
        /// <typeparam name="TData">The type of the data object to deserialise the JSON file into.</typeparam>
        /// <param name="fileName">Name of the file to parse.</param>
        /// <returns>An instance of <see cref="TData" />, populated with deserialised data from the JSON file.</returns>
        public static TData ParseJsonResourceAs<TData>(string fileName) where TData : new()
        {
            var json = new StringBuilder();
            var assembly = typeof(TData).Assembly;
            var stream = GetResourceStream(assembly, fileName);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream) json.AppendLine(reader.ReadLine());
            return JsonConvert.DeserializeObject<TData>(json.ToString());
        }

        /// <summary>
        ///     Parses the resource into a strongly-typed object.
        /// </summary>
        /// <typeparam name="TData">The type of the data object to deserialise the ProtoContract file into.</typeparam>
        /// <param name="fileName">Name of the file to parse.</param>
        /// <returns>An instance of <see cref="TData" />, populated with deserialised data from the ProtoContract file.</returns>
        public static TData ParseBinaryResourceAs<TData>(string fileName) where TData : class
        {
            var assembly = typeof(TData).Assembly;
            var stream = GetResourceStream(assembly, fileName);
            using var reader = new BinaryReader(stream);
            return ProtoEx.Deserialise<TData>(reader.ReadBytes((int)stream.Length));
        }

        /// <summary>
        ///     Determines whether an embedded resource exists within an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to find the resource in.</param>
        /// <param name="fileName">The name of the file to find.</param>
        /// <returns><c>true</c> if the embedded resource is found, <c>false</c> otherwise.</returns>
        public static bool ResourceExists(Assembly assembly, string fileName)
        {
            return assembly.GetManifestResourceNames().Any(p => p.EndsWith(fileName));
        }

        /// <summary>
        ///     Reads the resource, and passes back the output as a raw stream.
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="fileName">Name of the file, embedded within the assembly.</param>
        /// <returns>The contents of the file, as a raw stream.</returns>
        /// <exception cref="FileNotFoundException">Embedded data file not found.</exception>
        public static Stream GetResourceStream(Assembly assembly, string fileName)
        {
            var resource = assembly.GetManifestResourceNames().SingleOrDefault(p => p.EndsWith(fileName));
            if (string.IsNullOrWhiteSpace(resource))
                throw new MissingManifestResourceException($"Embedded data file not found: {fileName}");

            var stream = assembly.GetManifestResourceStream(resource);
            if (stream is null)
                throw new FileNotFoundException($"Embedded data file not found: {fileName}");

            return stream;
        }

        /// <summary>
        ///     Reads the resource, and passes back the output as a string
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="fileName">Name of the file, embedded within the assembly.</param>
        /// <returns>The contents of the file, as a string.</returns>
        /// <exception cref="FileNotFoundException">Embedded data file not found.</exception>
        public static string GetResourceContent(Assembly assembly, string fileName)
        {
            var stream = GetResourceStream(assembly, fileName);
            if (stream is null) return string.Empty;
            using var reader = new StreamReader(stream, Encoding.Default);
            return reader.ReadToEnd();
        }

        /// <summary>
        ///     Disembeds an embedded resource to specified location.
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="resourceName">The manifest name of the resource.</param>
        /// <param name="fileName">The full path to where the file should be copied to.</param>
        public static void DisembedResource(Assembly assembly, string resourceName, string fileName)
        {
            if (!ResourceExists(assembly, resourceName)) return;
            var stream64 = GetResourceStream(assembly, resourceName);
            using var file = File.OpenWrite(fileName);
            stream64.CopyTo(file);
        }

        /// <summary>
        ///     Disembeds embedded assets from a mod library, into the current mod's assets folder.
        /// </summary>
        /// <param name="assembly">The assembly to load the embedded assets from.</param>
        public static void DisembedAssets(Assembly assembly)
        {
            var resources = assembly.GetManifestResourceNames();
            var assemblyName = assembly.GetName().Name;

            foreach (var resource in resources)
            {
                if (!resource.StartsWith($"{assemblyName}.assets.")) continue;
                var str = resource.Replace($"{assemblyName}.assets.", "");
                var tmp = str.Split(new[] { '.' }, str.CountChars('.'));
                var fileInfo = new FileInfo(tmp.Aggregate(GamePathsEx.AssetsDirectory, Path.Combine));
                var stream64 = GetResourceStream(assembly, resource);
                Directory.CreateDirectory(fileInfo.DirectoryName!);
                using var file = File.Create(fileInfo.FullName);
                stream64.CopyTo(file);
            }
        }
    }
}