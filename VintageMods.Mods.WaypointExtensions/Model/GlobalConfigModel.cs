using ProtoBuf;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace VintageMods.Mods.WaypointExtensions.Model
{
    /// <summary>
    ///     Global configuration settings. Mainly used for version control.
    /// </summary>
    /// <remarks>
    ///     This class is intentionally left vague, to allow for scalability, as new features are implemented.
    /// </remarks>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class GlobalConfigModel
    {
        /// <summary>
        ///     The version of the currently installed mod.
        /// </summary>
        public string Version { get; set; }
    }
}