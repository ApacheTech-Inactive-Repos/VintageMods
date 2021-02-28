using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VintageMods.Core.Helpers.Enums;
using VintageMods.Core.Helpers.Resources;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VintageMods.Core.Helpers.Extensions
{
    /// <summary>
    ///     Extension Methods for the Core API.
    /// </summary>
	public static class CoreApiEx
    {
        public static string GetSeed(this ICoreAPI api)
        {
            return api?.World?.Seed.ToString();
		}


		// HACK:	The following methods would best be served within a dedicated FileManager class.
		//			For now, these will suffice, but as the project scales, these extension methods will
		//			need to become far more generic. They are already too specific, and the number of variables
		//			needed to parse directory and file info is becoming cumbersome. This can be addressed by
		//			Creating a FileManager system, and hooking that into the ModSystem as an when needed.
		//
		//			The file manager would know the mod name, and base directories. And would allow a mod to
		//			register files needed, so that they can be indexed, and managed easily.
		//
		//			FileManager.RegisterFile(FileType.Config, FileScope.Global, "global-config.data");


		public static T LoadOrCreateFile<T>(this ICoreAPI api, FileType fileType, string fileName, bool global = true) where T : class, new()
		{
			var fileInfo = new FileInfo(
				Path.Combine(new DirectoryInfo(
					Path.Combine(GamePaths.DataPath, fileType, "Waypoint Extensions", global ? "" : api.GetSeed())).FullName, fileName));

			var result = Activator.CreateInstance<T>();

			try
			{
				if (!fileInfo.Exists)
				{
                    Directory.CreateDirectory(fileInfo.DirectoryName ?? string.Empty);
					var contents = ResourceManager.ReadResourceRaw(typeof(T).Assembly, fileInfo.Name);
					File.WriteAllText(fileInfo.FullName, contents);
				}
				result = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileInfo.FullName));
			}
			catch (JsonException jsonException)
			{
				api.Logger.Error($"Failed reading contents of file ({fileInfo.Name}), error {jsonException.Message}.");
			}
			catch (IOException ioException)
			{
				api.Logger.Error($"Failed loading file ({fileInfo.Name}), error {ioException.Message}.");
			}

			return result;
		}

		public static List<T> PopulateFromFile<T>(this ICoreAPI api, string fileName, bool global = true) where T : class
		{
			var fileInfo = new FileInfo(Path.Combine(new DirectoryInfo(Path.Combine(GamePaths.DataPath, "ModData", "Waypoint Extensions", global ? "" : api.GetSeed())).FullName, fileName));
			var result = Activator.CreateInstance<List<T>>();
			try
			{
				if (!fileInfo.Exists)
				{
					Directory.CreateDirectory(fileInfo.DirectoryName ?? string.Empty);
					var contents = ResourceManager.ReadResourceRaw(typeof(T).Assembly, fileInfo.Name);
					File.WriteAllText(fileInfo.FullName, contents);
				}
				result = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(fileInfo.FullName));
			}
			catch (JsonException ex)
			{
				api.Logger.Error($"Failed reading contents of file ({fileInfo.Name}), error {ex.Message}.");
			}
			catch (IOException ex2)
			{
				api.Logger.Error($"Failed loading file ({fileInfo.Name}), error {ex2.Message}.");
			}
			return result;
		}

		public static void UpdateFile<T>(this ICoreAPI api, FileType fileType, string fileName, string modName, bool global = true) where T : class
		{
			var fileInfo = LocateFile(api, fileType, modName, fileName, global);

			try
			{
				Directory.CreateDirectory(fileInfo.DirectoryName ?? string.Empty);
				var contents = ResourceManager.ReadResourceRaw(typeof(T).Assembly, fileInfo.Name);
				File.WriteAllText(fileInfo.FullName, contents);
			}
			catch (Exception ex)
			{
				api.Logger.Error($"Failed loading file ({fileInfo.Name}), error {ex.Message}.");
			}
		}

		private static FileInfo LocateFile(ICoreAPI api, FileType fileType, string modName, string fileName, bool global)
		{
			var fileInfo =
				new FileInfo(Path.Combine(
					new DirectoryInfo(Path.Combine(GamePaths.DataPath, fileType, modName,
						global ? "" : api.GetSeed())).FullName, fileName));
			return fileInfo;
		}
	}
}