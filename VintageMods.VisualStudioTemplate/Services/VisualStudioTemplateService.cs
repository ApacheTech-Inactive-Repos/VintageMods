using VintageMods.Core.ModSystems.Client;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VintageMods.VisualStudioTemplate.Services
{
    public sealed class VisualStudioTemplateService : ClientSideService
    {
        private string _welcomeMessage;
        public override string RootFolder { get; } = "Visual Studio Template";

        public override void OnStart(ICoreClientAPI api)
        {
            base.OnStart(api);
            _welcomeMessage = $"Hello from the {RootFolder} mod.";
        }

        internal void SayHello(string name)
        {
            Api.ShowChatMessage($"{name}, {_welcomeMessage}");
        }

        public void OnSayHelloCommand(int groupid, CmdArgs args)
        {
            SayHello(args.PopWord("Vintarian"));
        }
    }
}