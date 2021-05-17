namespace VintageMods.Core.Legacy.Maths.Vectors
{
    public class Vec2d : Vector<double>
    {
        public Vec2d() : this(0, 0) { }

        public Vec2d(double x, double y) : base(2)
        {
            (X, Y) = (x, y);
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
    }
}