using VintageMods.Core.Threading.Systems;

namespace VintageMods.Core.Helpers
{
    public static class AsyncEx
    {
        public static ClientSystemAsyncActions Client { get; set; }
        public static ServerSystemAsyncActions Server { get; set; }
    }
}