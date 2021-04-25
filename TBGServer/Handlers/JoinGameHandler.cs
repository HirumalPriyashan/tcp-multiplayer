using System;
using Newtonsoft.Json.Linq;
using TBGServer.Enums;
using TBGServer.Helpers;
using TBGServer.Players;
using TBGServer.Packets;

namespace TBGServer.Handlers
{
    class JoinGameHandler : AbstractHandler
    {
        public override void Handle(Tuple<JObject, ClientPlayer> value)
        {
            MsgType msgType;
            JObject jObject = value.Item1;
            Enum.TryParse((string)jObject["type"], out msgType);
            if (msgType == MsgType.JoinGame)
            {
                JoinGamePacket packet = JsonSerialization.Deserialize<JoinGamePacket>(jObject);
                RoomManager.GetInstance().AddPlayerToGame(value.Item2,packet.gameId);
            }
            else
            {
                base.Handle(value);
            }
        }
    }
}