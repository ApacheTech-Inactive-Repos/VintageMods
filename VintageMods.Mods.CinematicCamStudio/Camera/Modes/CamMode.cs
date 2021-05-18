using System;
using System.Collections.Generic;
using VintageMods.Core.Reflection;
using VintageMods.Mods.CinematicCamStudio.Camera.Motion;
using VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Modes
{
    internal abstract class CamMode : ICamMode
    {
        protected ClientMain Game;
        protected ICoreClientAPI Api;

        public CamMode(ClientMain game, CamPath path)
        {
            _path = path;
            Default = new DefaultMode(game, path);
            LastPitch = path.Nodes[0].Pitch;
            LastYaw = path.Nodes[0].Yaw;
            Game = game;
            Api = (ICoreClientAPI)game.Api;
        }

        public static Dictionary<string, CamMode> Modes { get; } = new Dictionary<string, CamMode>();

        public static void RegisterPath(string id, CamMode path)
        {
            Modes.Add(id, path);
        }

        public static DefaultMode Default { get; private set; }


        public double LastYaw;
        public double LastPitch;
        private CamPath _path;

        public abstract CamMode Initialise(CamPath path);

        public CamNode GetPointBetween(CamNode point1, CamNode point2, double percent, double wholeProgress, float renderTickTime, bool isFirstLoop, bool isLastLoop)
        {
            var newPoint = _path.CachedInterpolation.GetPointInBetween(point1, point2, percent, wholeProgress, isFirstLoop, isLastLoop);
            if (_path.Target != null)
            {
                newPoint.Pitch = (float)LastPitch;
                newPoint.Yaw = (float)LastYaw;

                var pos = _path.Target.GetPosition();

                if (pos != null)
                {
                    var timeSinceLastRenderFrame = DateTime.Now.Ticks - CamEventHandlerClient.LastRenderTime;
                    newPoint.LookAt(pos, 0.00000001F, 0.00000001F, timeSinceLastRenderFrame / 600000000D * _path.CameraFollowSpeed);
                }
                LastPitch = newPoint.Pitch;
                LastYaw = newPoint.Y;
            }
            return newPoint;
        }

        public abstract void OnPathStart();

        public virtual void OnPathFinish()
        {
            ClientSettings.FieldOfView = 70;
            CamEventHandlerClient.Roll = 0;
        }

        public EntityPlayer GetCamera()
        {
            return Game.Player.Entity;
        }

        public abstract string GetDescription();

        public virtual void ProcessPoint(CamNode node)
        {
            ClientSettings.FieldOfView = (int)node.FieldOfView;

            Game.EntityPlayer.Pos.X = node.X;
            Game.EntityPlayer.Pos.Y = node.Y;
            Game.EntityPlayer.Pos.Z = node.Z;
            Game.EntityPlayer.Pos.Yaw = node.Yaw;
            Game.EntityPlayer.Pos.Pitch = node.Pitch;
            Game.EntityPlayer.Pos.Roll = node.Roll;

            Game.SetField("mouseYaw", node.Yaw);
            Game.SetField("mousePitch", node.Pitch);

            ClientSettings.FieldOfView = (int)node.FieldOfView;
            ClientSettings.SepiaLevel = (float)node.Sepia;

            ((AmbientManager)Api.Ambient).SetFogRange((float)node.FogLevel, 0);
            var c = ((AmbientManager) Api.Ambient).BlendedFogColor;
            ((AmbientManager)Api.Ambient).BlendedFogColor = 
                new Vec4f((float)node.FogColourR, (float)node.FogColourG, (float)node.FogColourB, c.A);
        }

        public static CamNode GetPoint(ICamMotion movement, List<CamNode> points, double percent, int currentLoop, int loops)
        {
            var lengthOfPoint = 1.0 / (points.Count - 1);
            var currentPoint = Math.Min((int)(percent / lengthOfPoint), points.Count - 2);
            var point1 = points[currentPoint];
            var point2 = points[currentPoint + 1];
            var percentOfPoint = percent % lengthOfPoint;
            return movement.GetPointInBetween(point1, point2, percentOfPoint, percent, currentLoop == 0, currentLoop == loops);
        }

        public static CamMode GetMode()
        {
            return Default;
        }
    }
}
