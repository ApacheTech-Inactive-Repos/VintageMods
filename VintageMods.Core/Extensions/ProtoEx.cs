using System.IO;
using ProtoBuf;

namespace VintageMods.Core.Extensions
{
    public static class ProtoEx
    {
        public static byte[] Serialise<T>(T record) where T : class
        {
            if (record == null) return null;
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, record);
            return stream.ToArray();
        }

        public static T Deserialise<T>(byte[] data) where T : class
        {
            if (data == null) return null;
            using var stream = new MemoryStream(data);
            return Serializer.Deserialize<T>(stream);
        }
    }
}