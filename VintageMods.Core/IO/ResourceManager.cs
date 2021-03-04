using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace VintageMods.Core.IO
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
        public static TData ParseResourceAs<TData>(string fileName) where TData : new()
        {
            var assembly = typeof(TData).Assembly;
            var json = ReadResourceRaw(assembly, fileName);
            return JsonConvert.DeserializeObject<TData>(json);
        }

        /// <summary>
        ///     Reads the resource, and passes back the output as a raw string.
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="fileName">Name of the file, embedded within the assembly.</param>
        /// <returns>The contents of the file, as a raw string.</returns>
        /// <exception cref="FileNotFoundException">Embedded data file not found.</exception>
        public static string ReadResourceRaw(Assembly assembly, string fileName)
        {
            var result = new StringBuilder();
            var text = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
            using (var stream = assembly.GetManifestResourceStream(text))
            {
                if (stream == null) throw new FileNotFoundException($"Embedded data file not found: {fileName}");
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream) result.AppendLine(streamReader.ReadLine());
                }
            }
            return result.ToString();
        }
    }
}