using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TBGServer.Enums;
using TBGServer.Packets;
using TBGServer.Players;
using TBGServer.Tcp;

namespace TBGServer.Handlers
{
    class HandlerManager
    {
        private IHandler<Tuple<JObject, ClientPlayer>> _handler;

        private IHandler<Tuple<JObject, TcpClient>> _loginHandler;
        private ConcurrentDictionary<string, ClientPlayer> _clientPlayerMap;

        public HandlerManager()
        {
            _clientPlayerMap = new ConcurrentDictionary<string, ClientPlayer>();
            CreateGameHandler createGameHandler = new CreateGameHandler();
            JoinGameHandler joinGameHandler = new JoinGameHandler();
            StartGameHandler startGameHandler = new StartGameHandler();
            GameMessageHandler gameMessageHandler = new GameMessageHandler();
            // TODO : set order according to traffic
            createGameHandler.SetNext(joinGameHandler).SetNext(startGameHandler).SetNext(gameMessageHandler);
            _handler = createGameHandler;
            LoginRequestHandler loginHandler = new LoginRequestHandler(this);
            _loginHandler = loginHandler;
        }
        public void Handle(JObject jObject, TcpClient client)
        {

            string token = null;
            token = (string)jObject["token"];
            if (token != null && token != "")
            {

                ClientPlayer clientPlayer = null;
                bool isExists = _clientPlayerMap.TryGetValue((string)jObject["token"], out clientPlayer);
                if (isExists)
                {
                    _handle(new Tuple<JObject, ClientPlayer>(jObject, clientPlayer));
                }
                else
                {
                    _handle(new Tuple<JObject, TcpClient>(jObject, client));
                }
            }
            else
            {
                _handle(new Tuple<JObject, TcpClient>(jObject, client));
            }
        }

        internal void AddToClientsMap(TcpClient client, string token)
        {
            _clientPlayerMap.TryAdd(token, new ClientPlayer(token, client));
        }

        private void _handle(Tuple<JObject, ClientPlayer> value)
        {
            _handler.Handle(value);
        }

        private void _handle(Tuple<JObject, TcpClient> value)
        {
            _loginHandler.Handle(value);
        }

    }
}