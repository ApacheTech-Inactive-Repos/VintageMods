using System;
using System.Collections.Generic;
using VintageMods.Core.Maths.Extensions;
using VintageMods.Core.Maths.Interpolation;
using VintageMods.Core.Maths.Matrix;
using VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding;
using VintageMods.Mods.CinematicCamStudio.Camera.Targetting;
using Vintagestory.API.MathTools;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Motion
{
    public class CircularMotion : HermiteMotion
    {
        private Vec3d _sphereOrigin;
        private double _radius;
        private CamTarget _target;
        private HermiteInterpolation _yAxis;

        public new Vec3d Colour { get; } = new Vec3d(1, 1, 0);

        internal new void Initialise(List<CamNode> points, int loops, CamTarget target)
        {
            if (target == null)
                throw new Exception("No target found");

            var center = target.GetPosition();

            if (center != null)
            {
                points.Add(points[0]);

                _target = target;
                var firstPoint = new Vec3d(points[0].X, points[0].Y, points[0].Z);
                var centerPoint = new Vec3d(center.X, center.Y, center.Z);
                _sphereOrigin = new Vec3d(firstPoint.X, firstPoint.Y, firstPoint.Z);

                _sphereOrigin.Sub(centerPoint);

                _radius = _sphereOrigin.Length();

                var vecs = new List<double>();
                var times = new List<double> { 0.0 };

                vecs.Add(firstPoint.Y);
                var newPointsSorted = new List<CamNode> { points[0] };

                for (var i = 1; i < points.Count - 1; i++)
                {

                    var point = new Vec3d(points[i].X, firstPoint.Y, points[i].Z);
                    point.Sub(centerPoint);

                    var dot = point.Dot(_sphereOrigin);
                    var det = ((point.X * _sphereOrigin.Z) - (point.Z * _sphereOrigin.X));
                    var angle = Math.Atan2(det, dot) * GameMath.RAD2DEG;

                    if (angle < 0)
                        angle += 360;

                    var time = angle / 360;
                    for (var j = 0; j < times.Count; j++)
                    {
                        if (times[j] > time)
                        {
                            times.Add(time);
                            vecs.Add(points[i].Y);
                            newPointsSorted.Add(points[i]);
                            break;
                        }
                    }
                    newPointsSorted.Add(points[i]);
                    times.Add(time);
                    vecs.Add(points[i].Y);
                }

                if (loops == 0)
                    newPointsSorted.Add(newPointsSorted[0].Clone());

                times.Add(1D);
                vecs.Add(firstPoint.Y);

                _yAxis = new HermiteInterpolation(times.ToArray(), vecs.ToArray());

                base.Initialise(times.ToArray(), newPointsSorted, loops, target);
            }
            else
                throw new Exception("Invalid target");
        }

        /// <summary>
        ///     Gets the point in between.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <param name="percent">The percent.</param>
        /// <param name="wholeProgress">The whole progress.</param>
        /// <param name="isFirstLoop">if set to <c>true</c> [is first loop].</param>
        /// <param name="isLastLoop">if set to <c>true</c> [is last loop].</param>
        /// <returns>CamNode.</returns>
        internal new CamNode GetPointInBetween(CamNode point1, CamNode point2, double percent, double wholeProgress, bool isFirstLoop,
            bool isLastLoop)
        {
            var newCamPoint = base.GetPointInBetween(point1, point2, percent, wholeProgress, isFirstLoop, isLastLoop);

            var angle = wholeProgress * 360;

            var center = _target.GetPosition();

            if (center == null) return newCamPoint;

            var centerPoint = new Vec3d(center.X, center.Y, center.Z);

            var newPoint = new Vec3d(_sphereOrigin.X, _sphereOrigin.Y, _sphereOrigin.Z) { Y = 0 };

            var matrix = new Matrix3d();
            Matrix3d.RotY(angle * GameMath.DEG2RAD);
            matrix.Transform(newPoint);

            newPoint.Y = _yAxis.ValueAt(wholeProgress) - center.Y;
            newPoint.Normalize();
            newPoint.Scale(_radius);

            newPoint.Add(centerPoint);
            newCamPoint.X = newPoint.X;
            newCamPoint.Y = newPoint.Y;
            newCamPoint.Z = newPoint.Z;
            return newCamPoint;
        }
    }
}