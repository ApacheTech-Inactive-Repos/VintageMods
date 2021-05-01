using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace VintageMods.Tools.ModPackager
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: packager.exe $(ProjectName) $(TargetDir) PackageDir");
                Environment.Exit(1);
                return;
            }

            var projectName = args[0];
            var targetDir = args[1];
            var packageDir = Path.Combine(args[1], args[2]);
            var modInfo = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(Path.Combine(targetDir, "modinfo.json")));
            var version = modInfo.GetProperty("version").GetString() ?? "1.0.0";
            var zipFilePath = Path.Combine(targetDir, $"{projectName}_v{version}.zip");

            Console.Write("Packaging Mod Archive... ");
            ZipFile.CreateFromDirectory(packageDir, zipFilePath);
            Console.WriteLine("[ SUCCESS ]");
        }
    }
}