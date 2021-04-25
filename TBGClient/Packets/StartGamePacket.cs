using Newtonsoft.Json;
using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class StartGamePacket : Packet
    {
        [JsonProperty("gameId")]
        public string gameId { get; private set; }
        [JsonProperty("token")]
        public string token { get; protected set; }
        public StartGamePacket(string id,string token, string msg = "") : base(MsgType.JoinGame, msg)
        {
            gameId = id;
            this.token = token;
        }
    }
}