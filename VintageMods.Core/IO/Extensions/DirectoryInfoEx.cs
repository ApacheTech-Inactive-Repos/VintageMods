﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Vintagestory.API.Config;

namespace VintageMods.Core.IO.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class DirectoryInfoEx
    {
        /// <summary>
        ///     Recursively deletes all files and sub-directories within a root directory; including the root directory itself.
        /// </summary>
        /// <param name="dir">The root directory to purge.</param>
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
                if (fi.Name.Equals(fileName))
                    return fi;
            foreach (var di in dir.GetDirectories()) RecursiveSearch(di, fileName);
            return null;
        }

        /// <summary>
        ///     Creates the specified directory, on the filesystem, if it doesn't already exist.
        /// </summary>
        /// <param name="di">The directory to create.</param>
        public static DirectoryInfo CreateIfNeeded(this DirectoryInfo di)
        {
            if (!di.Exists) di.Create();
            return di;
        }

        /// <summary>
        ///     Performs an action for each file in a directory.
        /// </summary>
        /// <param name="di">The directory that the files are in.</param>
        /// <param name="filter">A filter, to restrict the list of files within a directory.</param>
        /// <param name="action">The action to perform.</param>
        public static void WithFiles(this DirectoryInfo di, string filter, Action<FileInfo> action)
        {
            var files = di.GetFiles(filter);
            foreach (var file in files) action(file);
        }

        /// <summary>
        ///     Adds the directory to the Environment PATH variable.
        /// </summary>
        /// <param name="di">The director to add.</param>
        public static void AddToEnvironmentPath(this DirectoryInfo di)
        {
            var envSearchPathName = RuntimeEnv.EnvSearchPathName;
            var value = $"{di.FullName};{Environment.GetEnvironmentVariable(envSearchPathName)}";
            Environment.SetEnvironmentVariable(envSearchPathName, value);
        }

        /// <summary>
        ///     Returns a file list from the current directory matching the given set of extensions and
        ///     using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="di">The director to search.</param>
        /// <returns>An array of type <see cref="FileInfo" />.</returns>
        public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo di, SearchOption searchOption,
            params string[] extensions)
        {
            if (extensions == null) throw new ArgumentNullException(nameof(extensions));
            return extensions.SelectMany(p => di.GetFiles(p, searchOption));
        }
    }
}