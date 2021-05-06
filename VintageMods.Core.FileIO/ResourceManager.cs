using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using Newtonsoft.Json;
using VintageMods.Core.FileIO.Extensions;

namespace VintageMods.Core.FileIO
{
    /// <summary>
    ///     Controls access to embedded resources within assemblies,
    ///     allowing them to be parsed as strings or strongly-typed objects.
    /// </summary>
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
            return ProtoEx.Deserialise<TData>(reader.ReadBytes((int) stream.Length));
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
        ///     Reads the resource, and passes back the output as a raw string.
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="fileName">Name of the file, embedded within the assembly.</param>
        /// <returns>The contents of the file, as a raw string.</returns>
        /// <exception cref="FileNotFoundException">Embedded data file not found.</exception>
        public static Stream GetResourceStream(Assembly assembly, string fileName)
        {
            var resource = assembly.GetManifestResourceNames().SingleOrDefault(p => p.EndsWith(fileName));
            if (string.IsNullOrWhiteSpace(resource)) 
                throw new MissingManifestResourceException($"Embedded data file not found: {fileName}");

            var stream = assembly.GetManifestResourceStream(resource);
            if (stream == null) 
                throw new FileNotFoundException($"Embedded data file not found: {fileName}");

            return stream;
        }
    }
}