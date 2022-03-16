using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public class RegisterAccountResponse : IMessage
    {
        [ProtoMember(1)]
        public ResponseCode Code { get; set; }
    }
}
