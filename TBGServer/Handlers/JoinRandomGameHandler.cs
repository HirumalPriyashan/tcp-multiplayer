using System;
using Newtonsoft.Json.Linq;
using TBGServer.Enums;
using TBGServer.Helpers;
using TBGServer.Players;
using TBGServer.Packets;
using System.Collections.Generic;

namespace TBGServer.Handlers
{
    class JoinRandomGameHandler : AbstractHandler
    {
        public override void Handle(Tuple<JObject, ClientPlayer> value)
        {
            MsgType msgType;
            JObject jObject = value.Item1;
            Enum.TryParse((string)jObject["type"], out msgType);
            if (msgType == MsgType.JoinRandomGame)
            {
                JoinRandomGamePacket packet = JsonSerialization.Deserialize<JoinRandomGamePacket>(jObject);
                List<Room> games = RoomManager.GetInstance().GetGames();
                int index = new Random().Next(games.Count);
                string gameid = games[index].GetRoomId();
                RoomManager.GetInstance().AddPlayerToGame(value.Item2, gameid);
            }
            else
            {
                base.Handle(value);
            }
        }
    }
}