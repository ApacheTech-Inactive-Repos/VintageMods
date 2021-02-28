using Vintagestory.API.Common;

namespace VintageMods.Core.Helpers.Extensions
{
    public static class CmdArgsEx
    {
        public static string PopAll(this CmdArgs args, string defaultValue)
        {
            var retVal = args.PopAll();
            return string.IsNullOrWhiteSpace(retVal) ? defaultValue : retVal;
        }
    }
}