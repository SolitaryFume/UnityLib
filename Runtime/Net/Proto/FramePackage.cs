using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public sealed class FramePackage : IMessage
    {
        [ProtoMember(1)]
        public int tick { get; set; }
        [ProtoMember(2)]
        public FrameInfo[] data { get; set; }
    }
}