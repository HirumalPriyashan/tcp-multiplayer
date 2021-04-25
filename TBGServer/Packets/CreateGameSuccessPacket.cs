using Newtonsoft.Json;
using TBGServer.Enums;

namespace TBGServer.Packets
{
    internal class CreateGameSuccessPacket : Packet
    {
        [JsonProperty("gameId")]
        public string gameId{ get; private set; }
        public CreateGameSuccessPacket(string id, string msg = "") : base(MsgType.CreateGameSuccess, msg)
        {
            gameId = id;
        }
    }
}