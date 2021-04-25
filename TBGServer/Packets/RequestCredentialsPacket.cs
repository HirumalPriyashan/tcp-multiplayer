using TBGServer.Enums;

namespace TBGServer.Packets
{
    internal class RequestCredentialsPacket : Packet
    {
        public RequestCredentialsPacket(string msg = "") : base(MsgType.RequestCredentials, msg)
        {
        }
    }
}