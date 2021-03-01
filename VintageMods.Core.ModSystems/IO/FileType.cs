using StringEnum;

namespace VintageMods.Core.ModSystems.IO
{
    /// <summary>
    ///     Specifies the folder a file is saved to, within the user's game data folder.
    /// </summary>
    public class FileType : StringEnum<FileType>
    {
        public static readonly FileType Config = Create("ModConfig");
        public static readonly FileType Data = Create("ModData");
    }
}