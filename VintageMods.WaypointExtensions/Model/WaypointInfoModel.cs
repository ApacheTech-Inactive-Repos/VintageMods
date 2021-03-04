using System.Collections.Generic;
using JetBrains.Annotations;

namespace VintageMods.WaypointExtensions.Model
{
    /// <summary>
    ///     A model of all information required to set a Waypoint.
    /// </summary>
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, 
        ImplicitUseTargetFlags.WithMembers)]
    public sealed class WaypointInfoModel
    {
        /// <summary>
        ///     Gets the list of syntax arguments that are valid for this waypoint type.
        /// </summary>
        /// <value>The list of syntax arguments that are valid for this waypoint type.</value>
        public List<string> Syntax { get; set; } = new List<string>();

        /// <summary>
        ///     Gets the icon to use for the waypoint.
        /// </summary>
        /// <value>The icon to use for the waypoint.</value>
        public string Icon { get; set; }

        /// <summary>
        ///     Gets the colour to use for the waypoint marker.
        /// </summary>
        /// <value>The colour to use for the waypoint marker.</value>
        public string Colour { get; set; }

        /// <summary>
        ///     Gets the default title of the waypoint marker.
        /// </summary>
        /// <value>The default title of the waypoint marker.</value>
        public string DefaultTitle { get; set; }
    }
}