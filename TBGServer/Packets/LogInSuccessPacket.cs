using Newtonsoft.Json;
using TBGServer.Enums;

namespace TBGServer.Packets
{
    internal class LogInSuccessPacket : Packet
    {
        [JsonProperty("token")]
        public string _token{ get; private set; }
        public LogInSuccessPacket(string token, string msg = "") : base(MsgType.LogInSuccess, msg)
        {
            _token = token;
        }
    }
}