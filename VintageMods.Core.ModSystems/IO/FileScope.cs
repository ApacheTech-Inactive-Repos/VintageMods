using StringEnum;

namespace VintageMods.Core.ModSystems.IO
{
    /// <summary>
    ///     Specifies the scope of a file saved to the user's game folder.
    /// </summary>
    public class FileScope : StringEnum<FileScope>
    {
        public static readonly FileScope Global = Create("Global");
        public static readonly FileScope World = Create("World");
    }
}