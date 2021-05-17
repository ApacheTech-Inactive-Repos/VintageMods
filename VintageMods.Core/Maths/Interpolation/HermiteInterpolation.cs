using VintageMods.Core.Maths.Enum;

namespace VintageMods.Core.Maths.Interpolation
{
    public class HermiteInterpolation : InterpolationBase
    {
        private readonly double _bias;
        private readonly HermiteTension _tension;

        public HermiteInterpolation(double[] times, double[] points, double bias, HermiteTension tension) : base(times, points)
        {
            _bias = bias;
            _tension = tension;
        }

        public HermiteInterpolation(double bias, HermiteTension tension, params double[] points) : base(points)
        {
            _bias = bias;
            _tension = tension;
        }

        public HermiteInterpolation(double[] times, double[] points,
            HermiteTension tension = HermiteTension.Normal) : this(times, points, 0, tension)
        {
        }

        public HermiteInterpolation(HermiteTension tension, params double[] points) : this(0, tension, points)
        {
        }

        public HermiteInterpolation(params double[] points) : this(HermiteTension.Normal, points)
        {
        }

        public override double ValueAt(double mu, int pointIndex, int pointIndexNext)
        {
            var v0 = GetValue(pointIndex - 1);
            var v1 = GetValue(pointIndex);
            var v2 = GetValue(pointIndexNext);
            var v3 = GetValue(pointIndexNext + 1);

            var mu2 = mu * mu;
            var mu3 = mu2 * mu;
            var m0 = (v1 - v0) * (1 + _bias) * (1 - (int) _tension) / 2;
            m0 += (v2 - v1) * (1 - _bias) * (1 - (int) _tension) / 2;
            var m1 = (v2 - v1) * (1 + _bias) * (1 - (int) _tension) / 2;
            m1 += (v3 - v2) * (1 - _bias) * (1 - (int) _tension) / 2;
            var a0 = 2 * mu3 - 3 * mu2 + 1;
            var a1 = mu3 - 2 * mu2 + mu;
            var a2 = mu3 - mu2;
            var a3 = -2 * mu3 + 3 * mu2;

            return a0 * v1 + a1 * m0 + a2 * m1 + a3 * v2;
        }
    }
}