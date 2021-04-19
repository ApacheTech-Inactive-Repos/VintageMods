using System;
using VintageMods.Core.FileIO.Enum;
using Vintagestory.API.Common;

namespace VintageMods.Core.FileIO.Extensions
{
    public static class ApiExtensions
    {
        private static FileManager _fileManagerInstance;

        public static FileManager RegisterFileManager(this ICoreAPI api, string rootFolder, 
            params (string FileName, FileType FileType, FileScope FileScope)[] files)
        {
            _fileManagerInstance = new FileManager(api, rootFolder);
            foreach (var (fileName, fileType, fileScope) in files)
            {
                _fileManagerInstance.RegisterFile(fileName, fileType, fileScope);
            }

            return _fileManagerInstance;
        }

        public static FileManager FileManager(this ICoreAPI api, string rootFolder = null)
        {
            if (string.IsNullOrEmpty(rootFolder)) return _fileManagerInstance;
            return _fileManagerInstance ??= new FileManager(api, rootFolder);
        }

        public static ModFileInfo RegisterModConfigFile(this ICoreAPI api, string fileName, FileScope fileScope)
        {
            try
            {
                return _fileManagerInstance.RegisterConfigFile(fileName, fileScope);
            }
            catch (Exception e)
            {
                api.Logger.Error(e.Message);
                throw;
            }
        }

        public static ModFileInfo RegisterModDataFile(this ICoreAPI api, string fileName, FileScope fileScope)
        {
            try
            {
                return _fileManagerInstance.RegisterDataFile(fileName, fileScope);
            }
            catch (Exception e)
            {
                api.Logger.Error(e.Message);
                throw;
            }
        }

        public static ModFileInfo GetModFile(this ICoreAPI api, string fileName)
        {
            try
            {
                return _fileManagerInstance.ModFiles[fileName];
            }
            catch (Exception e)
            {
                api.Logger.Error(e.Message);
                throw;
            }
        }
    }
}