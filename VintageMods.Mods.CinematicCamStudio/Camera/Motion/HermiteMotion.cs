using System.Collections.Generic;
using VintageMods.Core.Maths.Enum;
using VintageMods.Core.Maths.Interpolation;
using VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding;
using VintageMods.Mods.CinematicCamStudio.Camera.Targetting;
using VintageMods.Mods.CinematicCamStudio.Exceptions;
using Vintagestory.API.MathTools;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Motion
{
    public class HermiteMotion : ICamMotion
    {
        private HermiteInterpolation _xSpline;
        private HermiteInterpolation _ySpline;
        private HermiteInterpolation _zSpline;
        private HermiteInterpolation _yawSpline;
        private HermiteInterpolation _pitchSpline;
        private HermiteInterpolation _rollSpline;
        private HermiteInterpolation _fovSpline;
        private HermiteInterpolation _saturationSpline;
        private HermiteInterpolation _sepiaSpline;

        private double _sizeOfIteration;

        public Vec3d Colour { get; } = new Vec3d(1, 1, 1);
        public void Initialise(List<CamNode> points, int loops, CamTarget target)
        {
            Initialise(null, points, loops, target);
        }

        protected void Initialise(double[] times, List<CamNode> points, int loops, CamTarget target)
        {
            if (points.Count == 1)
                throw new CamStudioException("At least two points are required");

            var iterations = loops == 0 ? 1 : loops == 1 ? 2 : 3;

            _sizeOfIteration = 1D / iterations;

            var size = points.Count * iterations;

            if (iterations > 1)
                size++;

            var xPoints = new double[size];
            var yPoints = new double[size];
            var zPoints = new double[size];

            var yawPoints = new double[size];
            var pitchPoints = new double[size];
            var rollPoints = new double[size];

            var fovPoints = new double[size];
            var saturationPoints = new double[size];
            var sepiaPoints = new double[size];

            var newTimes = new double[size];

            for (var j = 0; j < iterations; j++)
            {
                if (times != null)
                {
                    for (var i = 0; i < times.Length; i++)
                    {
                        var index = i + points.Count * j;
                        if (index < size)
                            newTimes[index] = times[i] * _sizeOfIteration + _sizeOfIteration * j;
                    }
                }
                for (var i = 0; i < points.Count; i++)
                {
                    xPoints[i + j * points.Count] = points[i].X;
                    yPoints[i + j * points.Count] = points[i].Y;
                    zPoints[i + j * points.Count] = points[i].Z;

                    yawPoints[i + j * points.Count] = points[i].Yaw;
                    pitchPoints[i + j * points.Count] = points[i].Pitch;
                    rollPoints[i + j * points.Count] = points[i].Roll;

                    fovPoints[i + j * points.Count] = points[i].FieldOfView;
                    saturationPoints[i + j * points.Count] = points[i].Saturation;
                    sepiaPoints[i + j * points.Count] = points[i].Sepia;
                }

                if (iterations > 1)
                {
                    xPoints[points.Count * iterations] = points[0].X;
                    yPoints[points.Count * iterations] = points[0].Y;
                    zPoints[points.Count * iterations] = points[0].Z;

                    yawPoints[points.Count * iterations] = points[0].Yaw;
                    pitchPoints[points.Count * iterations] = points[0].Pitch;
                    rollPoints[points.Count * iterations] = points[0].Roll;

                    fovPoints[points.Count * iterations] = points[0].FieldOfView;
                    saturationPoints[points.Count * iterations] = points[0].Saturation;
                    sepiaPoints[points.Count * iterations] = points[0].Sepia;
                }

                _xSpline = new HermiteInterpolation(HermiteTension.Normal, xPoints);
                _ySpline = new HermiteInterpolation(HermiteTension.Normal, yPoints);
                _zSpline = new HermiteInterpolation(HermiteTension.Normal, zPoints);

                if (times == null)
                {
                    _yawSpline = new HermiteInterpolation(yawPoints);
                    _pitchSpline = new HermiteInterpolation(pitchPoints);
                    _rollSpline = new HermiteInterpolation(rollPoints);

                    _fovSpline = new HermiteInterpolation(fovPoints);
                    _saturationSpline = new HermiteInterpolation(saturationPoints);
                    _sepiaSpline = new HermiteInterpolation(sepiaPoints);
                }
                else
                {
                    _yawSpline = new HermiteInterpolation(newTimes, yawPoints);
                    _pitchSpline = new HermiteInterpolation(newTimes, pitchPoints);
                    _rollSpline = new HermiteInterpolation(newTimes, rollPoints);

                    _fovSpline = new HermiteInterpolation(newTimes, fovPoints);
                    _saturationSpline = new HermiteInterpolation(newTimes, saturationPoints);
                    _sepiaSpline = new HermiteInterpolation(newTimes, sepiaPoints);
                }
            }
        }

        public CamNode GetPointInBetween(CamNode point1, CamNode point2, double percent, double wholeProgress, bool isFirstLoop,
            bool isLastLoop)
        {
            var point = point1.GetPointBetween(point2, percent);

            var iteration = isFirstLoop ? 0 : isLastLoop && _sizeOfIteration < 0.5 ? 2 : 1;
            var additionalProgress = iteration * _sizeOfIteration;
            wholeProgress = additionalProgress + wholeProgress * _sizeOfIteration;


            if (_xSpline != null)
                point.X = _xSpline.ValueAt(wholeProgress);

            if (_ySpline != null)
                point.Y = _ySpline.ValueAt(wholeProgress);

            if (_zSpline != null)
                point.Z = _zSpline.ValueAt(wholeProgress);


            if (_yawSpline != null)
                point.Yaw = (float)_yawSpline.ValueAt(wholeProgress);

            if (_pitchSpline != null)
                point.Pitch = (float)_pitchSpline.ValueAt(wholeProgress);

            if (_rollSpline != null)
                point.Roll = (float)_rollSpline.ValueAt(wholeProgress);


            if (_fovSpline != null)
                point.FieldOfView = _fovSpline.ValueAt(wholeProgress);

            if (_saturationSpline != null)
                point.Saturation = _saturationSpline.ValueAt(wholeProgress);

            if (_sepiaSpline != null)
                point.Sepia = _sepiaSpline.ValueAt(wholeProgress);

            return point;
        }
    }
}