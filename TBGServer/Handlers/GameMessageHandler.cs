using System;
using Newtonsoft.Json.Linq;
using TBGServer.Enums;
using TBGServer.Helpers;
using TBGServer.Players;
using TBGServer.Packets;

namespace TBGServer.Handlers
{
    class GameMessageHandler : AbstractHandler
    {
        public override void Handle(Tuple<JObject, ClientPlayer> value)
        {
            MsgType msgType;
            JObject jObject = value.Item1;
            Enum.TryParse((string)jObject["type"], out msgType);
            if (msgType == MsgType.GameMessage)
            {
                GameMessagePacket packet = JsonSerialization.Deserialize<GameMessagePacket>(jObject);
                RoomManager.GetInstance().AddMessageToGame(packet,packet.gameId);
            }
            else
            {
                base.Handle(value);
            }
        }
    }
}