using System;

namespace VintageMods.Core.Maths.Interpolation
{
    public class CosineInterpolation : InterpolationBase
    {
        public CosineInterpolation(double[] times, double[] points) : base(times, points)
        {
        }

        public CosineInterpolation(params double[] points) : base(points)
        {
        }

        public override double ValueAt(double mu, int pointIndex, int pointIndexNext)
        {
            var mu2 = (1 - Math.Cos(mu * Math.PI)) / 2;
            return GetValue(pointIndex) * (1 - mu2) + GetValue(pointIndexNext) * mu2;
        }
    }
}