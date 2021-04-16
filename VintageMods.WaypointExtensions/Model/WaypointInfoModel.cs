using System.Collections.Generic;
using ProtoBuf;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace VintageMods.WaypointExtensions.Model
{
    /// <summary>
    ///     A model of all information required to set a Waypoint.
    /// </summary>
    [ProtoContract]
    public sealed class WaypointInfoModel
    {
        /// <summary>
        ///     Gets the list of syntax arguments that are valid for this waypoint type.
        /// </summary>
        /// <value>The list of syntax arguments that are valid for this waypoint type.</value>
        [ProtoMember(1)]
        public List<string> Syntax { get; set; } = new List<string>();

        /// <summary>
        ///     Gets the icon to use for the waypoint.
        /// </summary>
        /// <value>The icon to use for the waypoint.</value>
        [ProtoMember(2)]
        public string Icon { get; set; }

        /// <summary>
        ///     Gets the colour to use for the waypoint marker.
        /// </summary>
        /// <value>The colour to use for the waypoint marker.</value>
        [ProtoMember(3)]
        public string Colour { get; set; }

        /// <summary>
        ///     Gets the default title of the waypoint marker.
        /// </summary>
        /// <value>The default title of the waypoint marker.</value>
        [ProtoMember(4)]
        public string DefaultTitle { get; set; }
    }
}