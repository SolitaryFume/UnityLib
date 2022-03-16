
namespace Proto
{
    public enum ProtoID : ushort
    {
        // ERROR = -1,
        RegisterAccountRequest,
        RegisterAccountResponse,
        LoginRequest,
        LoginResponse,

        JoinRoomRequest,
        JoinRoomResponse,

        FrameInfo,
        FramePackage,

        StartRoomSync,

        MAX
    }
}
