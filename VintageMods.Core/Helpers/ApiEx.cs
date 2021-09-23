using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageMods.Core.Helpers
{
    public static class ApiEx
    {
        public static ICoreAPI Universal { get; set; }
        public static ICoreClientAPI Client { get; set; }
        public static ICoreServerAPI Server { get; set; }
    }
}