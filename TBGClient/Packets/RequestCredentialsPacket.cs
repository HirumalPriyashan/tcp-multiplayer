using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class RequestCredentialsPacket : Packet
    {
        public RequestCredentialsPacket(string msg = "") : base(MsgType.RequestCredentials, msg)
        {
        }
    }
}