using System;
using VintageMods.CinematicCamStudio.Camera.Pathfinding;
using VintageMods.CinematicCamStudio.Camera.Targetting;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace VintageMods.CinematicCamStudio.Camera.Modes
{
    internal class DefaultMode : CamMode
    {
        private ClientWorldPlayerData _prevWData;

        public DefaultMode(ClientMain game, CamPath path) : base (game, path)
        {
            if (path?.Target?.GetType() == typeof(CamTargetSelf))
                path.Target = null;
        }

        public override CamMode Initialise(CamPath path)
        {
            return new DefaultMode(Game, path);
        }

        public override void ProcessPoint(CamNode point)
        {
            base.ProcessPoint(point);
        }

        public override void OnPathStart()
        {
            ClientSettings.AmbientSoundLevel = 0;
            ClientSettings.SoundLevel = 0;
            ClientSettings.WeatherSoundLevel = 0;
            ClientSettings.MusicLevel = 0;
            ClientSettings.WavingFoliage = false;

            var worlddata = Game.Player.WorldData as ClientWorldPlayerData;
            _prevWData = worlddata?.Clone();
            worlddata?.RequestMode(Game, worlddata.MoveSpeedMultiplier, worlddata.PickingRange, EnumGameMode.Spectator, true, true, EnumFreeMovAxisLock.None);
        }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}