using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public enum ResponseCode : ushort
    {
        OK,
        ERROR,
    }
}