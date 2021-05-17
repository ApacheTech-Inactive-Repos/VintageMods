namespace VintageMods.Core.Legacy.Maths.Vectors
{
    public class Vec3d : Vector<double>
    {
        public Vec3d() : this(0,0,0) { }

        public Vec3d(double x, double y, double z) : base(3)
        {
            (X, Y, Z) = (x, y, z);
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
    }
}