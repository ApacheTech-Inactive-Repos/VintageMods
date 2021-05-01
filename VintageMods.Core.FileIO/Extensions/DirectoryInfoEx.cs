using System.IO;

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
    }
}