using StringEnum;

namespace VintageMods.Core.Helpers.Enums
{
    public class FileType : StringEnum<FileType>
    {
        public static readonly FileType Config = Create("ModConfig");
        public static readonly FileType Data = Create("ModData");
    }
}