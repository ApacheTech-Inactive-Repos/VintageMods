using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace VintageMods.Core.FileIO.Extensions
{
    public static class ProtoEx
    {
        public static IEnumerable<byte> Serialise<T>(T record) where T : class
        {
            if (null == record) return null;
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, record);
            return stream.ToArray();
        }

        public static T Deserialise<T>(IEnumerable<byte> data) where T : class
        {
            if (null == data) return null;
            using var stream = new MemoryStream(data.ToArray());
            return Serializer.Deserialize<T>(stream);
        }
    }
}