using System;
using System.Linq;
using System.Reflection;
using VintageMods.Core.Attributes;
using VintageMods.Core.IO.Enum;
using Vintagestory.API.Common;

// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.IO.Extensions
{
    public static class ApiExtensions
    {
        private static FileManager _fileManagerInstance;

        public static FileManager RegisterFileManager(this ICoreAPI api, 
            params (string FileName, FileScope FileScope)[] files)
        {
            var rootFolder = Assembly.GetCallingAssembly().GetCustomAttributes()
                .OfType<ModDomainAttribute>().FirstOrDefault()?.RootFolder ?? "Common";

            _fileManagerInstance = new FileManager(api, rootFolder);
            foreach (var (fileName, fileScope) in files)
            {
                _fileManagerInstance.RegisterFile(fileName, fileScope);
            }

            return _fileManagerInstance;
        }

        public static FileManager FileManager(this ICoreAPI api, string rootFolder = null)
        {
            if (string.IsNullOrEmpty(rootFolder)) return _fileManagerInstance;
            return _fileManagerInstance ??= new FileManager(api, rootFolder);
        }

       public static ModFileInfo GetModFile(this ICoreAPI api, string fileName)
        {
            try
            {
                return _fileManagerInstance.ModFiles[fileName];
            }
            catch (Exception e)
            {
                api.Logger.Audit($"[VintageMods] Error occurred locating file. Check Error Log.");
                api.Logger.Error($"[VintageMods] {e.Message}");
                throw;
            }
        }
    }
}