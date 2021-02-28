using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace VintageMods.Core.Helpers.Resources
{
    public static class ResourceManager
    {
		public static TData ParseResourceAs<TData>(string fileName) where TData : new()
		{
            var assembly = typeof(TData).Assembly;
            var json = ReadResourceRaw(assembly, fileName);
			return JsonConvert.DeserializeObject<TData>(json);
        }

        public static string ReadResourceRaw(Assembly assembly, string fileName)
        {
            var result = new StringBuilder();

            var text = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));

            using (var stream = assembly.GetManifestResourceStream(text))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Embedded data file not found: " + fileName);
                }
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        result.AppendLine(streamReader.ReadLine());
                    }
                }
            }

            return result.ToString();
        }
    }
}