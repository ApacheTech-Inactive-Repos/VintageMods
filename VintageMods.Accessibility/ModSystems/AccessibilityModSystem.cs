using System.Text;
using HarmonyLib;
using VintageMods.Core.Client.ModSystems;
using Vintagestory.API.Client;

namespace VintageMods.Accessibility.ModSystems
{
    internal class AccessibilityModSystem : ClientSideModSystem
    {
        private const string PatchCode = "VintageMods.Accessibility";
        private readonly Harmony _harmonyInstance = new Harmony(PatchCode);

        public override void StartClientSide(ICoreClientAPI api)
        {
            _harmonyInstance.PatchAll();
            var builder = new StringBuilder("Harmony Patched Methods: ");
            foreach (var val in _harmonyInstance.GetPatchedMethods())
            {
                builder.Append(val.Name + ", ");
            }
            api.Logger.Notification(builder.ToString());
        }

        public override void Dispose()
        {
            _harmonyInstance.UnpatchAll(PatchCode);
        }
    }
}