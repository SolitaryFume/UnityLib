using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Proto
{

    public static class ProtoHelp
    {
        private static Dictionary<ProtoID, Type> protocols;
        private static Dictionary<Type, ProtoID> keys;

        static ProtoHelp()
        {
            var names = Enum.GetNames(typeof(ProtoID));
            protocols = new Dictionary<ProtoID, Type>(names.Length);
            keys = new Dictionary<Type, ProtoID>();
            for (int i = 0; i < names.Length-1; i++) //排除max
            {
                var key = (ProtoID)Enum.Parse(typeof(ProtoID), names[i]);
                var tyName = $"Proto.{names[i]}";
                var value = Type.GetType(tyName);
                if (value == null)
                {
                    throw new ProtoException($"No Find Type:{tyName} !");
                }
                else
                {
                    protocols.Add(key, value);
                    keys.Add(value, key);
                }
            }
        }

        public static IMessage Decoder(ReadOnlySpan<byte> data)
        {
            var id = (ProtoID)BitConverter.ToUInt16(data);
            using var stream = new MemoryStream(data.Slice(2).ToArray());
            return Serializer.Deserialize(protocols[id], stream) as IMessage;
        }

        public static ReadOnlySpan<byte> Encoder(IMessage message)
        {
            var ty = message.GetType();
            if (!keys.TryGetValue(ty, out var id))
            {
                throw new ProtoException($"Enum ProtoID No Find Name {ty.Name} !");
            }
            using var memory = new MemoryStream();
            Serializer.Serialize(memory, message);
            var data = new byte[memory.Length + 6];
            Array.Copy(BitConverter.GetBytes(data.Length), 0, data, 0, 4);
            Array.Copy(BitConverter.GetBytes((ushort)id), 0, data,4,2);
            Array.Copy(memory.ToArray(), 0, data, 6, memory.Length);
            return data;
        }

        public static IMessage Decoder(ushort proroid, byte[] data)
        {
            if (proroid > (int)ProtoID.MAX)
            {
                throw new ProtoException($"Enum ProtoID No Find Value {proroid} !");
            }
            var id = (ProtoID)proroid;
            using var stream = new MemoryStream(data);
            return Serializer.Deserialize(protocols[id], stream) as IMessage;
        }
    }
}