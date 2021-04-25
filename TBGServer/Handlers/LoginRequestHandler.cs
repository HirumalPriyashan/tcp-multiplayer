using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TBGServer.Enums;
using TBGServer.Helpers;
using TBGServer.Packets;
using TBGServer.Tcp;

namespace TBGServer.Handlers
{
    internal class LoginRequestHandler : IHandler<Tuple<JObject, TcpClient>>
    {
        private HandlerManager _handlerManager;

        public LoginRequestHandler(HandlerManager handlerManager)
        {
            _handlerManager = handlerManager;
        }

        public void Handle(Tuple<JObject, TcpClient> value)
        {
            MsgType msgType;
            JObject jObject = value.Item1;
            TcpClient client = value.Item2;
            List<Task> loginTasks = new List<Task>();

            Enum.TryParse((string)jObject["type"], out msgType);
            if (msgType == MsgType.Credentials)
            {
                loginTasks.Add(_handleLoginRequest(JsonSerialization.Deserialize<CredentialsPacket>(jObject), client));
            }
            else
            {
                string msg = "Send your credentials";
                string token = null;
                token = (string)jObject["token"];
                if (token != null)
                {
                    msg = "Invalid token";
                }
                loginTasks.Add(_requestLoginCred(client, msg));
            }
        }

        public IHandler<Tuple<JObject, TcpClient>> SetNext(IHandler<Tuple<JObject, TcpClient>> handler)
        {
            throw new NotImplementedException();
        }
        private async Task _requestLoginCred(TcpClient client, string msg)
        {
            await TcpSender.SendPacket(client, new RequestCredentialsPacket(msg));
        }

        private async Task _handleLoginRequest(CredentialsPacket packet, TcpClient client)
        {
            // TODO: need to handle with database
            if (packet.username == "asdf")
            {
                string token = "Guid.NewGuid().ToString()";
                _handlerManager.AddToClientsMap(client, token);
                await TcpSender.SendPacket(client, new LogInSuccessPacket(token));
            }
            else
            {
                await TcpSender.SendPacket(client, new LogInFailPacket("Invalid username or password"));
            }
        }
    }
}