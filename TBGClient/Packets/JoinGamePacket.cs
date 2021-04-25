using Newtonsoft.Json;
using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class JoinGamePacket : Packet
    {
        [JsonProperty("gameId")]
        public string gameId{ get; private set; }
        public JoinGamePacket(string id, string msg = "") : base(MsgType.JoinGame, msg)
        {
            gameId = id;
        }
    }
}