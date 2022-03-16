using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public class FrameInfo : IMessage
    {
        [ProtoMember(1)]
        public int tick { get; set; }
        [ProtoMember(2)]
        public ushort direction { get; set; }

        public override string ToString()
        {
            return $"tick:{tick},direction:{direction}";
        }
    }
}