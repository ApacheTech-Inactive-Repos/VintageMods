namespace VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding
{
    public static class GameMaths
    {
        public static float WrapDegrees(float value)
        {
            value %= 360.0F;

            if (value >= 180.0F)
            {
                value -= 360.0F;
            }

            if (value < -180.0F)
            {
                value += 360.0F;
            }

            return value;
        }
        public static double WrapDegrees(double value)
        {
            value %= 360.0F;

            if (value >= 180.0F)
            {
                value -= 360.0F;
            }

            if (value < -180.0F)
            {
                value += 360.0F;
            }

            return value;
        }
    }
}