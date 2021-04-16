using System.Collections.Generic;
using VintageMods.CinematicCamStudio.Camera.Pathfinding;
using VintageMods.CinematicCamStudio.Camera.Targetting;
using Vintagestory.API.MathTools;

namespace VintageMods.CinematicCamStudio.Camera.Motion
{
    public interface ICamMotion
    {
        Vec3d Colour { get; }

        void Initialise(List<CamNode> points, int loops, CamTarget target);

        CamNode GetPointInBetween(CamNode point1, CamNode point2, double percent, double wholeProgress, bool isFirstLoop, bool isLastLoop);
    }
}