using System.Collections.Generic;
using System.Linq;
using VintageMods.Core.Maths.Interpolation;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding
{
    internal class CamSpline<T>
    {
        private List<T> _points;

        private IInterpolator _interpolator;

        public CamSpline(IEnumerable<T> points, IInterpolator interpolator)
        {
            _points = points.ToList();
            _interpolator = interpolator;
        }
    }
}
