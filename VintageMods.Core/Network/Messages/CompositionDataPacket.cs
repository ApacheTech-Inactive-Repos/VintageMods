using ProtoBuf;

namespace VintageMods.Core.Network.Messages
{
    /// <summary>
    ///     Represents a network packet used for MEF composition.
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class CompositionDataPacket
    {
        /// <summary>
        ///     Gets or sets the mod-id.
        /// </summary>
        /// <value>The mod-id used to send the request.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the requested composition data.
        /// </summary>
        /// <value>The requested composition data.</value>
        public byte[] Data { get; set; }
    }
}