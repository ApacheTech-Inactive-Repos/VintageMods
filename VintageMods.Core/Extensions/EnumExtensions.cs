using System.ComponentModel;
using JetBrains.Annotations;

namespace VintageMods.Core.Extensions
{
    /// <summary>
    ///     Contains extension methods for working with enums.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the description for the enum member, decorated with a DescriptionAttribute.
        /// </summary>
        public static string GetDescription(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}