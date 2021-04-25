using TBGServer.Enums;

namespace TBGServer.Packets
{
    internal class JoinRandomGamePacket : Packet
    {
        public JoinRandomGamePacket(string msg = "") : base(MsgType.JoinRandomGame, msg)
        {
        }
    }
}