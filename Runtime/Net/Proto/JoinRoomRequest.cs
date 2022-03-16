using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public sealed class JoinRoomRequest : IMessage
    {
        [ProtoMember(1)]
        public ulong userid { get; set; }
    }
}