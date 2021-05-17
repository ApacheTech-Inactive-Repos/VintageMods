namespace VintageMods.Core.Legacy.Maths.Vectors
{
    public class Vec4d : Vector<double>
    {
        public Vec4d() : this(0, 0, 0, 0) { }

        public Vec4d(double x, double y, double z, double w) : base(4)
        {
            (X, Y, Z, W) = (x, y, z, w);
        }

        public double X
        {
            get => Values[0];
            set => Values[0] = value;
        }

        public double Y
        {
            get => Values[1];
            set => Values[1] = value;
        }

        public double Z
        {
            get => Values[2];
            set => Values[2] = value;
        }

        public double W
        {
            get => Values[3];
            set => Values[3] = value;
        }
    }
}