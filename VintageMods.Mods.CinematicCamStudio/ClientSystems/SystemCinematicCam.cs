using VintageMods.Core.Extensions;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

// ReSharper disable RedundantArgumentDefaultValue

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
#pragma warning disable 649

namespace VintageMods.Mods.CinematicCamStudio.ClientSystems
{
    internal class SystemCinematicCam : ClientSystem
    {
        private readonly ClientMain _game;
        private readonly ICoreClientAPI _capi;

        private MeshData _cameraPathModel;
        private MeshRef _cameraPathModelRef;
        private readonly ClientPlatformAbstract _platform;
        private BlockPos _origin;

        public SystemCinematicCam(ClientMain game) : base(game)
        {
            _game = game;
            _capi = (ICoreClientAPI)game.Api;

            InitModel();
            _platform = game.GetField<ClientPlatformAbstract>("Platform");

            _capi.UnregisterCommand("cam");
            _capi.RegisterCommand("cam", "Cinematic Camera Studio", "", CmdCam);
            
            var eventManager = game.GetField<ClientEventManager>("eventManager");
            eventManager.CallMethod("RegisterRenderer", new DummyRenderer { action = OnRenderFrame3D }, EnumRenderStage.Opaque, "cinecam", 0.7f);
            eventManager.CallMethod("RegisterRenderer", new DummyRenderer { action = OnFinalizeFrame }, EnumRenderStage.Done, "cinecam-done", 0.98f);
        }

        private void CmdCam(int groupid, CmdArgs args)
        {
            if (_game.Player.WorldData.CurrentGameMode != EnumGameMode.Creative && _game.Player.WorldData.CurrentGameMode != EnumGameMode.Spectator)
            {
                _game.ShowChatMessage("Only available in Creative, or Spectator Mode.");
                return;
            }
            
            var option = args.PopWord("");

            switch (option)
            {
                case "test":
                    // TODO: Perform an action.
                    // TODO: Feedback to user.
                    break;
            }
        }

        void InitModel()
        {
            _cameraPathModel = new MeshData(4, 4, false, false, true, true);
            _cameraPathModel.SetMode(EnumDrawMode.LineStrip);
            _cameraPathModelRef = null;
        }

        public void OnRenderFrame3D(float dt)
        {
            if (!_game.GetField<bool>("ShouldRender2DOverlays") || _cameraPathModelRef == null) return;

            var prog = ShaderPrograms.Autocamera;

            prog.Use();
            _platform.CallMethod("GLLineWidth", 2);
            _platform.CallMethod("BindTexture2d", 0);

            _game.GlPushMatrix();
            _game.GlLoadMatrix(_game.GetField<PlayerCamera>("MainCamera").GetField<double[]>("CameraMatrixOrigin"));


            var cameraPos = _game.EntityPlayer.CameraPos;
            _game.GlTranslate(
                (float)(_origin.X - cameraPos.X),
                (float)(_origin.Y - cameraPos.Y),
                (float)(_origin.Z - cameraPos.Z)
            );

            prog.ProjectionMatrix = _game.CurrentProjectionMatrix;
            prog.ModelViewMatrix = _game.CurrentModelViewMatrix;

            _platform.CallMethod("RenderMesh", _cameraPathModelRef);

            _game.GlPopMatrix();

            prog.Stop();
        }

        public void OnFinalizeFrame(float dt)
        {

        }

        public override EnumClientSystemType GetSystemType()
        {
            return EnumClientSystemType.Misc;
        }

        public override string Name => "cica";

    }
}
