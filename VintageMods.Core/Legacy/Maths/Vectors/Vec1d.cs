namespace VintageMods.Core.Legacy.Maths.Vectors
{
    public class Vec1d : Vector<double>
    {
        public Vec1d() : this(0) { }

        public Vec1d(double x) : base(1)
        {
            X = x;
        }

        public double X
        {
            get => Values[0];
            set => Values[0] = value;
        }
    }
}