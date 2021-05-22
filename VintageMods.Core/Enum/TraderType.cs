

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

namespace VintageMods.Core.Enum
{    public sealed class TraderType : StringEnum<TraderType>
    {
        public static string Artisan = Create("artisan");
        public static string BuildingSupplies = Create("buildmaterials");
        public static string Clothing = Create("clothing");
        public static string Commodities = Create("commodities");
        public static string Foods = Create("foods");
        public static string Furniture = Create("furniture");
        public static string Luxuries = Create("luxuries");
        public static string SurvivalGoods = Create("survivalgoods");
        public static string TreasureHunter = Create("treasurehunter");
    }
}