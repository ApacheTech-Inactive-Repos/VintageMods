using System;
using System.Collections.Generic;
using VintageMods.Core.Common.Reflection;
using VintageMods.Mods.CinematicCamStudio.Camera.Modes;
using VintageMods.Mods.CinematicCamStudio.Camera.Motion;
using VintageMods.Mods.CinematicCamStudio.Camera.Targetting;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

#pragma warning disable 649

namespace VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding
{
    internal class CamPath
    {
        public List<CamNode> Nodes { get; }

        public List<CamNode> TempNodes { get; private set; }

        private readonly int _loop;
        private TimeSpan _duration;
        private readonly string _mode;
        private readonly string _interpolation;
        internal CamTarget Target;
        internal readonly double CameraFollowSpeed;
        private bool _serverPath;

        private bool _hideGui;

        internal ICamMotion CachedInterpolation;

        private CamMode _cachedMode;

        private DateTime _timeStarted;
        private int _currentLoop;

        public bool HasFinished { get; private set; }
        public bool IsRunning { get; private set; }

        // TODO: How to get rendertick length? This may have to be a ClientSystem, and use OnRenderTick().
        // 
        // Physics ticks are set by GlobalConstants static class.
        private float _renderTickTime;

        public CamPath(int loop, TimeSpan duration, string mode, string interpolation, CamTarget target, List<CamNode> points, double cameraFollowSpeed)
        {
            _loop = loop;
            _duration = duration;
            _mode = mode;
            _interpolation = interpolation;
            Target = target;
            Nodes = points;
            CameraFollowSpeed = cameraFollowSpeed;
        }

        public void OverwriteClientConfig()
        {
            CamPathSettings.LastLoop = _loop;
            CamPathSettings.LastDuration = _duration;
            CamPathSettings.LastMode = _mode;
            CamPathSettings.LastInterpolation = _interpolation;
            CamPathSettings.Target = Target;
            CamPathSettings.Points = new List<CamNode>(Nodes);
            CamPathSettings.CameraFollowSpeed = CameraFollowSpeed;
        }

        public void Start(IClientWorldAccessor world)
        {
            HasFinished = false;
            IsRunning = true;

            _timeStarted = DateTime.Now;
            _currentLoop = 0;
            TempNodes = new List<CamNode>(Nodes);
            if (_loop != 0)
                TempNodes.Add(TempNodes[TempNodes.Count - 1].Clone());


            var parser = CamMode.GetMode();
            _cachedMode = parser.Initialise(this);
            _cachedMode.OnPathStart();

            CachedInterpolation = CamMotionFactory.CreateInstance(_interpolation);
            CachedInterpolation.Initialise(TempNodes, _loop, Target);

            _hideGui = (world as ClientMain).GetField<bool>("ShouldRender2DOverlays");

        }
        
        public void Finish(IClientWorldAccessor world)
        {
            HasFinished = true;
            IsRunning = false;

            _cachedMode.OnPathFinish();
            TempNodes = null;

            _cachedMode = null;
            CachedInterpolation = null;

            (world as ClientMain).SetField("ShouldRender2DOverlays", _hideGui);
        }

        public void Tick(IClientWorldAccessor world, float dt)
        {
            var time = DateTime.Now - _timeStarted;
            if (time >= _duration)
            {
                if (_currentLoop < _loop || _loop < 0)
                {
                    _timeStarted = DateTime.Now;
                    _currentLoop++;
                }
                else
                    Finish(world);
            }
            else
            {
                (world as ClientMain).SetField("ShouldRender2DOverlays", true);

                long durationOfPoint = _duration.Milliseconds / (TempNodes.Count - 1);
                var currentPoint = Math.Min((int)(time.Milliseconds / durationOfPoint), TempNodes.Count - 2);
                var point1 = TempNodes[currentPoint];
                var point2 = TempNodes[currentPoint + 1];
                var percent = (time.Milliseconds % durationOfPoint) / (double)durationOfPoint;
                var newPoint = _cachedMode.GetPointBetween(point1, point2, percent, (double)time.Milliseconds / _duration.Milliseconds,
                    _renderTickTime, _currentLoop == 0, _currentLoop == _loop);

                if (newPoint != null)
                    _cachedMode.ProcessPoint(newPoint);

            }
        }

        public CamPath Clone()
        {
            return new CamPath(_currentLoop, _duration, _mode, _interpolation, Target, new List<CamNode>(Nodes),
                CameraFollowSpeed)
            { _serverPath = _serverPath };
        }
    }
}