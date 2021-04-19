using System;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Motion
{
    public static class CamMotionFactory
    {
        public static ICamMotion Linear => new LinearMotion();
        public static ICamMotion Cosine => new SmoothMotion();
        public static ICamMotion Cubic => new CubicMotion();
        public static ICamMotion CubicCatmull => new CubicCatmullMotion();
        public static ICamMotion Hermite => new HermiteMotion();
        public static ICamMotion Circular => new CircularMotion();
        public static ICamMotion Smooth => new SmoothMotion();

        public static ICamMotion CreateInstance(string type)
        {
            switch (type.ToLowerInvariant())
            {
                case "linear": return Linear;
                case "cosine": return Cosine;
                case "Cubic": return Cubic;
                case "CubicCatmull": return CubicCatmull;
                case "Hermite": return Hermite;
                case "Circular": return Circular;
                case "Smooth": return Smooth;
                default:
                    throw new ArgumentException("Invalid motion type.", nameof(type));
            }
        }
    }
}