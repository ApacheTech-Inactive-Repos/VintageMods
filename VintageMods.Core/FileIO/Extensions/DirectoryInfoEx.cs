using System;
using System.IO;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace VintageMods.Core.FileIO.Extensions
{
    public static class DirectoryInfoEx
    {
        public static void RecursivePurge(this DirectoryInfo dir)
        {
            foreach (var fi in dir.GetFiles()) fi.Delete();
            foreach (var di in dir.GetDirectories()) di.RecursivePurge();
            dir.Delete();
        }


        /// <summary>
        ///     Searches for a file within the currently selected directory, and all child directories.
        /// </summary>
        /// <param name="dir">The root directory to perform the search from.</param>
        /// <param name="fileName">The name of the file to search for, including file extension.</param>
        /// <returns>Returns a FileInfo instance pertaining to the file, if it is found, otherwise returns null.</returns>
        public static FileInfo RecursiveSearch(this DirectoryInfo dir, string fileName)
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

        public static DirectoryInfo CreateIfNeeded(this DirectoryInfo di)
        {
            if (!di.Exists) di.Create();
            return di;
        }

        public static void WithFiles(this DirectoryInfo di, string filter, Action<FileInfo> action)
        {
            var files = di.GetFiles(filter);
            foreach (var file in files)
            {
                action(file);
            }
        }

        public static void LoadAssemblies(this DirectoryInfo di)
        {
            di.WithFiles("*.dll", file => AssemblyEx.LoadLibrary(file.FullName));
        }

        public static void AddToEnvironmentPath(this DirectoryInfo di)
        {
            var envSearchPathName = RuntimeEnv.EnvSearchPathName;
            var value = $"{di.FullName};{Environment.GetEnvironmentVariable(envSearchPathName)}";
            Environment.SetEnvironmentVariable(envSearchPathName, value);
        }
    }
}