using Vintagestory.API.MathTools;

namespace VintageMods.Core.Maths.Extensions
{
    public static class Vec3dEx
    {
        public static Vec3d Scale(this Vec3d vec, double scaleFactor)
        {
            return new(vec.X * scaleFactor, vec.Y * scaleFactor, vec.Z * scaleFactor);
        }
    }
}