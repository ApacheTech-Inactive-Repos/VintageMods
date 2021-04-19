using System.Collections.Generic;
using VintageMods.Core.Maths.Interpolation;
using VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding;
using VintageMods.Mods.CinematicCamStudio.Camera.Targetting;
using VintageMods.Mods.CinematicCamStudio.Exceptions;
using Vintagestory.API.MathTools;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Motion
{
    public class CubicCatmullMotion : ICamMotion
    {
        private CubicCatmullInterpolation _xSpline;
        private CubicCatmullInterpolation _ySpline;
        private CubicCatmullInterpolation _zSpline;
        private CubicCatmullInterpolation _yawSpline;
        private CubicCatmullInterpolation _pitchSpline;
        private CubicCatmullInterpolation _rollSpline;
        private CubicCatmullInterpolation _fovSpline;
        private CubicCatmullInterpolation _saturationSpline;
        private CubicCatmullInterpolation _sepiaSpline;

        private double _sizeOfIteration;

        public Vec3d Colour { get; } = new Vec3d(1, 0, 0);

        public void Initialise(List<CamNode> points, int loops, CamTarget target)
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

            for (var j = 0; j < iterations; j++)
            {
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

            _xSpline = new CubicCatmullInterpolation(xPoints);
            _ySpline = new CubicCatmullInterpolation(yPoints);
            _zSpline = new CubicCatmullInterpolation(zPoints);

            _yawSpline = new CubicCatmullInterpolation(yawPoints);
            _pitchSpline = new CubicCatmullInterpolation(pitchPoints);
            _rollSpline = new CubicCatmullInterpolation(rollPoints);

            _fovSpline = new CubicCatmullInterpolation(fovPoints);
            _saturationSpline = new CubicCatmullInterpolation(saturationPoints);
            _sepiaSpline = new CubicCatmullInterpolation(sepiaPoints);
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