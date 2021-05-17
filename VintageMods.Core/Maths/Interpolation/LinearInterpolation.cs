namespace VintageMods.Core.Maths.Interpolation
{
    public class LinearInterpolation : InterpolationBase
    {
        public LinearInterpolation(double[] times, double[] points) : base(times, points)
        {
        }

        public LinearInterpolation(params double[] points) : base(points)
        {
        }

        public override double ValueAt(double mu, int pointIndex, int pointIndexNext)
        {
            return GetValue(pointIndexNext) - GetValue(pointIndex) * mu + GetValue(pointIndex);
        }
    }
}