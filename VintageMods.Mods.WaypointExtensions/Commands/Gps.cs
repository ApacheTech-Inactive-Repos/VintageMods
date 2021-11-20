using VintageMods.Core.Extensions;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Extensions;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.WaypointExtensions.Mod.Commands
{
    [FluentChatCommand("gps")]
    public class Gps : FluentChatCommandBase<ICoreClientAPI>
    { 
        public Gps(ICoreClientAPI api) : base(api) { }
        
        public override void OnNoOption(string option, CmdArgs args)
        {
            var player = Api.World.Player;
            var pos = player.Entity.Pos.AsBlockPos.RelativeToSpawn(Api.World);
            Api.SendChatMessage(LangEx.Message("BroadcastGPS", player.PlayerName, pos.X, pos.Y, pos.Z));
            Api.ShowChatMessage(LangEx.Message("LocationCopiedToClipboard"));
            Api.Forms.SetClipboardText($"X = {pos.X}, Y = {pos.Y}, Z = {pos.Z}.");
        }
    }
}
