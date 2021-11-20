using ApacheTech.WaypointExtensions.Mod.ModSystems;
using VintageMods.Core.Extensions;
using VintageMods.Core.FluentChat.Extensions;
using VintageMods.Core.Helpers;
using VintageMods.Core.IO.Extensions;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace ApacheTech.WaypointExtensions.Mod.UI
{
    internal class SettingsWindow : GuiDialog
    {
        private ICoreClientAPI Api { get; }
        private WpexModSystem Mod { get; }

        public override string ToggleKeyCombinationCode => "wpex-settings";
        public override bool PrefersUngrabbedMouse => false;

        public SettingsWindow(ICoreClientAPI api) : base(api)
        {
            Api = api;
            Mod = api.ModLoader.GetModSystem<WpexModSystem>();
            ComposeWindow();
        }

        internal void ComposeWindow()
        {
            ClearComposers();

            var rows = new[]{ 0f, 1f, 1.5f, 2.5f, 3.0f };
            const float width = 500f;

            var guiComposer = Api.AsClientMain().GetField<GuiComposerManager>("GuiComposers")
                .Create("wpex-settings", ElementStdBounds.AutosizedMainDialog)
                .AddShadedDialogBG(ElementStdBounds.DialogBackground()
                    .WithFixedPadding(GuiStyle.ElementToDialogPadding, GuiStyle.ElementToDialogPadding), false)
                .BeginChildElements();

            // Title.
            guiComposer.AddStaticText(LangEx.UI("SettingsWindow.Headers.Wpex"),
                CairoFont.WhiteSmallishText().WithFontSize(25f),
                EnumTextOrientation.Center,
                ElementStdBounds.MenuButton(rows[0])
                    .WithFixedWidth(width));

            // Auto Translocator.
            guiComposer.AddStaticText(LangEx.UI("SettingsWindow.Labels.AutoTranslocator"), CairoFont.WhiteSmallishText(), 
                    ElementStdBounds.MenuButton(rows[1], EnumDialogArea.LeftTop).WithFixedWidth(330))
                .AddHoverText(LangEx.UI("SettingsWindow.Tooltips.AutoTranslocator"), CairoFont.WhiteSmallText(), 250,
                    ElementStdBounds.MenuButton(rows[1], EnumDialogArea.LeftTop).WithFixedWidth(250))
                .AddSwitch(AutoTranslocatorChanged,
                    ElementStdBounds.MenuButton(rows[1], EnumDialogArea.LeftTop).WithFixedOffset(350.0, 0), "AutoTranslocatorWaypoints");

            // Auto Trader
            guiComposer.AddStaticText(LangEx.UI("SettingsWindow.Labels.AutoTrader"), CairoFont.WhiteSmallishText(),
                    ElementStdBounds.MenuButton(rows[2], EnumDialogArea.LeftTop).WithFixedWidth(330))
                .AddHoverText(LangEx.UI("SettingsWindow.Tooltips.AutoTrader"), CairoFont.WhiteSmallText(), 250,
                    ElementStdBounds.MenuButton(rows[2], EnumDialogArea.LeftTop).WithFixedWidth(250))
                .AddSwitch(AutoTraderChanged, 
                    ElementStdBounds.MenuButton(rows[2], EnumDialogArea.LeftTop).WithFixedOffset(350.0, 0), "AutoTraderWaypoints");

            // Donate Button.
            guiComposer.AddButton(LangEx.UI("SettingsWindow.Labels.Donate"), OnDonateButton,
                ElementStdBounds.MenuButton(rows[3]).WithFixedWidth(width));

            // Back Button.
            guiComposer.AddButton(Lang.Get("pause-back2game"), OnBackToGame,
                ElementStdBounds.MenuButton(rows[4]).WithFixedWidth(width));

            guiComposer.GetSwitch("AutoTranslocatorWaypoints").On = Mod.Settings.AutoTranslocatorWaypoints;
            guiComposer.GetSwitch("AutoTraderWaypoints").On = Mod.Settings.AutoTraderWaypoints;

            SingleComposer = guiComposer.EndChildElements().Compose();
        }

        private void AutoTranslocatorChanged(bool state)
        {
            Mod.Settings.AutoTranslocatorWaypoints = state;
            SaveSettings();
        }

        private void AutoTraderChanged(bool state)
        {
            Mod.Settings.AutoTraderWaypoints = state;
            SaveSettings();
        }

        private bool OnDonateButton()
        {
            CrossPlatformHelpers.OpenBrowser("https://bit.ly/APGDonate");
            return true;
        }

        private bool OnBackToGame()
        {
            SaveSettings();
            TryClose();
            return true;
        }

        private void SaveSettings()
        {
            Api.GetModFile("wpex-settings.data").SaveAsJson(Mod.Settings);
        }
    }
}
