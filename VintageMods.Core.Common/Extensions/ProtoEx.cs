using System.IO;
using ProtoBuf;

namespace VintageMods.Core.Common.Extensions
{
    public static class ProtoEx
    {
        public static byte[] Serialise<T>(T record) where T : class
        {
            if (null == record) return null;
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, record);
                return stream.ToArray();
            }
        }

        public static T Deserialise<T>(byte[] data) where T : class
        {
            if (null == data) return null;
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
    }
}