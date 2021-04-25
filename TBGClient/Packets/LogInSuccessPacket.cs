using Newtonsoft.Json;
using TBGClient.Enums;

namespace TBGClient.Packets
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