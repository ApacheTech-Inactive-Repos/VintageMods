namespace VintageMods.Core.Maths.Interpolation
{
    public class CubicCatmullInterpolation : InterpolationBase
    {
        private readonly double _beginVec;
        private readonly double _endVec;

        public CubicCatmullInterpolation(double[] times, double[] points) : base(times, points)
        {
            _beginVec = points[0] + (points[0] - points[1]);
            _endVec = points[points.Length - 1] + (points[points.Length - 1] - (points[points.Length - 2]));
        }

        public CubicCatmullInterpolation(params double[] points) : base(points)
        {
            _beginVec = points[0] + (points[0] - points[1]);
            _endVec = points[points.Length - 1] + (points[points.Length - 1] - (points[points.Length - 2]));
        }

        protected override double GetValue(int index)
        {
            if (index < 0)
            {
                return _beginVec;
            }
            return index >= Points.Count
                ? _endVec
                : PointVecs[index];
        }

        public override double ValueAt(double mu, int pointIndex, int pointIndexNext)
        {
            var v0 = GetValue(pointIndex - 1);
            var v1 = GetValue(pointIndex);
            var v2 = GetValue(pointIndexNext);
            var v3 = GetValue(pointIndexNext + 1);

            var mu2 = mu * mu;
            var a0 = -0.5 * v0 + 1.5 * v1 - 1.5 * v2 + 0.5 * v3;
            var a1 = v0 - 2.5 * v1 + 2 * v2 - 0.5 * v3;
            var a2 = -0.5 * v0 + 0.5 * v2;
            var a3 = v1;

            return a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3;
        }
    }
}