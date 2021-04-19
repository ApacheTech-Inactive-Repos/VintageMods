using Vintagestory.API.Common;

namespace VintageMods.Mods.WaypointExtensions.Database
{
    public class DbContext : SQLiteDBConnection
    {
        public DbContext(ILogger logger) : base(logger)
        {

		}
    }
}
