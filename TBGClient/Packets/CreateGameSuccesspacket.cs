using Newtonsoft.Json;
using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class CreateGameSuccessPacket : Packet
    {
        [JsonProperty("gameId")]
        public string gameId{ get; private set; }
        public CreateGameSuccessPacket(string id, string msg = "") : base(MsgType.CreateGame, msg)
        {
            gameId = id;
        }
    }
}