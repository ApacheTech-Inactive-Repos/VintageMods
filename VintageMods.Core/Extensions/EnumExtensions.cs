using System.ComponentModel;

namespace VintageMods.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}