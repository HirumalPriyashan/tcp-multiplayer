using Newtonsoft.Json;
using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class CreateGamePacket : Packet
    {
        [JsonProperty("gameName")]
        public string gameName { get; private set; }
        [JsonProperty("gameType")]
        public GameType gameType { get; private set; }
        [JsonProperty("token")]
        public string token { get; protected set; }
        public CreateGamePacket(string name, GameType gType, string token, string msg = "") : base(MsgType.CreateGame, msg)
        {
            gameName = name;
            gameType = gType;
            this.token = token;
        }
    }
}