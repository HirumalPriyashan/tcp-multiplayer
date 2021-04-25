using Newtonsoft.Json;
using TBGServer.Enums;

namespace TBGServer.Packets
{
    internal class LogInFailPacket : Packet
    {
        public LogInFailPacket(string msg = "") : base(MsgType.LogInFail, msg)
        {
        }
    }
}