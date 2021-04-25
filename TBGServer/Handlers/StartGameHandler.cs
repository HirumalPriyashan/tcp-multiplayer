using System;
using Newtonsoft.Json.Linq;
using TBGServer.Enums;
using TBGServer.Helpers;
using TBGServer.Players;
using TBGServer.Packets;

namespace TBGServer.Handlers
{
    class StartGameHandler : AbstractHandler
    {
        public override void Handle(Tuple<JObject, ClientPlayer> value)
        {
            MsgType msgType;
            JObject jObject = value.Item1;
            Enum.TryParse((string)jObject["type"], out msgType);
            if (msgType == MsgType.StartGame)
            {
                StartGamePacket packet = JsonSerialization.Deserialize<StartGamePacket>(jObject);
                RoomManager.GetInstance().StartGame(packet.gameId,value.Item2);
            }
            else
            {
                base.Handle(value);
            }
        }
    }
}