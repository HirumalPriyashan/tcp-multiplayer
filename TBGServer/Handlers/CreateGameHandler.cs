using System;
using Newtonsoft.Json.Linq;
using TBGServer.Enums;
using TBGServer.Helpers;
using TBGServer.Players;
using TBGServer.Packets;

namespace TBGServer.Handlers
{
    class CreateGameHandler : AbstractHandler
    {
        public override void Handle(Tuple<JObject, ClientPlayer> value)
        {
            MsgType msgType;
            JObject jObject = value.Item1;
            Enum.TryParse((string)jObject["type"], out msgType);
            if (msgType == MsgType.CreateGame)
            {
                CreateGamePacket packet = JsonSerialization.Deserialize<CreateGamePacket>(jObject);
                RoomManager.GetInstance().CreateGame(value.Item2, packet.gameName, packet.gameType);
            }
            else
            {
                base.Handle(value);
            }
        }
    }
}