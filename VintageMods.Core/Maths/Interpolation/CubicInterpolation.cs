namespace VintageMods.Core.Maths.Interpolation
{
    public class CubicInterpolation : InterpolationBase
    {
        private readonly double _beginVec;
        private readonly double _endVec;

        public CubicInterpolation(double[] times, double[] points) : base(times, points)
        {
            _beginVec = points[0] + (points[0] - points[1]);
            _endVec = points[points.Length - 1] + (points[points.Length - 1] - (points[points.Length - 2]));
        }

        public CubicInterpolation(params double[] points) : base(points)
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
                : PointVectors[index];
        }

        public override double ValueAt(double mu, int pointIndex, int pointIndexNext)
        {
            var v0 = GetValue(pointIndex - 1);
            var v1 = GetValue(pointIndex);
            var v2 = GetValue(pointIndexNext);
            var v3 = GetValue(pointIndexNext + 1);

            var mu2 = mu * mu;
            var a0 = v3 - v2 - v0 + v1;
            var a1 = v0 - v1 - a0;
            var a2 = v2 - v0;
            var a3 = v1;

            return a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3;
        }
    }
}