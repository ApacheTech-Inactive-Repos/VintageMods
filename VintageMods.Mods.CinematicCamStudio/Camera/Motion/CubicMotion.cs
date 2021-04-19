using System.Collections.Generic;
using VintageMods.Core.Maths.Interpolation;
using VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding;
using VintageMods.Mods.CinematicCamStudio.Camera.Targetting;
using VintageMods.Mods.CinematicCamStudio.Exceptions;
using Vintagestory.API.MathTools;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Motion
{
    public class CubicMotion : ICamMotion
    {
        private CubicInterpolation _xSpline;
        private CubicInterpolation _ySpline;
        private CubicInterpolation _zSpline;
        private CubicInterpolation _yawSpline;
        private CubicInterpolation _pitchSpline;
        private CubicInterpolation _rollSpline;
        private CubicInterpolation _fovSpline;
        private CubicInterpolation _saturationSpline;
        private CubicInterpolation _sepiaSpline;

        public double SizeOfIteration;

        public Vec3d Colour { get; } = new Vec3d(1, 0, 0);

        public void Initialise(List<CamNode> points, int loops, CamTarget target)
        {
            if (points.Count == 1)
                throw new CamStudioException("At least two points are required");

            var iterations = loops == 0 ? 1 : loops == 1 ? 2 : 3;

            SizeOfIteration = 1D / iterations;

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

            _xSpline = new CubicInterpolation(xPoints);
            _ySpline = new CubicInterpolation(yPoints);
            _zSpline = new CubicInterpolation(zPoints);

            _yawSpline = new CubicInterpolation(yawPoints);
            _pitchSpline = new CubicInterpolation(pitchPoints);
            _rollSpline = new CubicInterpolation(rollPoints);

            _fovSpline = new CubicInterpolation(fovPoints);
            _saturationSpline = new CubicInterpolation(saturationPoints);
            _sepiaSpline = new CubicInterpolation(sepiaPoints);
        }

        public CamNode GetPointInBetween(CamNode point1, CamNode point2, double percent, double wholeProgress, bool isFirstLoop,
            bool isLastLoop)
        {
            var point = point1.GetPointBetween(point2, percent);

            var iteration = isFirstLoop ? 0 : isLastLoop && SizeOfIteration < 0.5 ? 2 : 1;
            var additionalProgress = iteration * SizeOfIteration;
            wholeProgress = additionalProgress + wholeProgress * SizeOfIteration;


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