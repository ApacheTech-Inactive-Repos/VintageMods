namespace VintageMods.Core.Extensions
{
    public static class BooleanExtensions
    {
        public static void Toggle(this ref bool state)
        {
            state = !state;
        }
    }
}