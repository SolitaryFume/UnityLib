using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public sealed class JoinRoomResponse : IMessage
    {
        [ProtoMember(1)]
        public int code { get; set; }
    }
}