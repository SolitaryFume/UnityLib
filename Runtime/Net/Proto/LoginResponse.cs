using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public class LoginResponse : IMessage
    {
        [ProtoMember(1)]
        public ResponseCode Code { get; set; }
    }
}
