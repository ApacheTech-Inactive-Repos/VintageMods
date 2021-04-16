using System.Collections.Generic;
using VintageMods.CinematicCamStudio.Camera.Pathfinding;
using VintageMods.CinematicCamStudio.Camera.Targetting;
using Vintagestory.API.MathTools;

namespace VintageMods.CinematicCamStudio.Camera.Motion
{
    public class LinearMotion : ICamMotion
    {
        public Vec3d Colour { get; } = new Vec3d(0, 0, 1);

        public void Initialise(List<CamNode> points, int loops, CamTarget target)
        {
            
        }

        public CamNode GetPointInBetween(CamNode point1, CamNode point2, double percent, double wholeProgress, bool isFirstLoop,
            bool isLastLoop)
        {
            return point1.GetPointBetween(point2, percent);
        }
    }
}