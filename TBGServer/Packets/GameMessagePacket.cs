using Newtonsoft.Json;
using TBGServer.Enums;

namespace TBGServer.Packets
{
    internal class GameMessagePacket : Packet
    {
        [JsonProperty("gameId")]
        public string gameId{ get; private set; }
        public GameMessagePacket(string id, string msg = "") : base(MsgType.JoinGame, msg)
        {
            gameId = id;
        }
    }
}