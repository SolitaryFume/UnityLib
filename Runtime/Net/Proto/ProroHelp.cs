using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace Proto
{
    public static class ProroHelp
    {
        private static Dictionary<ProtoID, Type> protocols;
        private static Dictionary<Type, ProtoID> keys;

        static ProroHelp()
        {
            var names = Enum.GetNames(typeof(ProtoID));
            protocols = new Dictionary<ProtoID, Type>(names.Length);
            keys = new Dictionary<Type, ProtoID>();
            for (int i = 0; i < names.Length; i++)
            {
                var key = (ProtoID)Enum.Parse(typeof(ProtoID), names[i]);
                var value = Type.GetType($"Proto.{names[i]}");
                if (value == null)
                {
                    
                }
                else
                {
                    protocols.Add(key, value);
                    keys.Add(value,key);
                }   
            }
        }

        public static IMessage Decoder(Span<byte> data)
        {
            var id = (ProtoID)BitConverter.ToUInt16(data);
            using (var stream = new MemoryStream())
            {
                stream.Write(data.Slice(2));
                return Serializer.Deserialize(protocols[id], stream) as IMessage;
            }
        }

        public static byte[] Encoder(IMessage message)
        {
            var ty = message.GetType();
            var id = keys[ty];
            using (var memory = new MemoryStream())
            {
                Serializer.Serialize(memory, message);
                var data = new byte[memory.Length + 4];
                Array.Copy(BitConverter.GetBytes((ushort)data.Length), 0, data, 0, 2);
                Array.Copy(BitConverter.GetBytes((ushort)id), 0, data, 2, 2);
                Array.Copy(memory.ToArray(), 0, data, 4,memory.Length);
                return data;
            }
        }

        public static IMessage Decoder(ushort proroid,byte[] data)
        {
            var id = (ProtoID)proroid;
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize(protocols[id], stream) as IMessage;
            }
        }
    }
}