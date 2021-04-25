using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class LogInFailPacket : Packet
    {
        public LogInFailPacket(string msg = "") : base(MsgType.LogInFail, msg)
        {
        }
    }
}