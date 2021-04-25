using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class JoinRandomGamePacket : Packet
    {
        public JoinRandomGamePacket(string msg = "") : base(MsgType.JoinRandomGame, msg)
        {
        }
    }
}