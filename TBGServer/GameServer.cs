using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TBGServer.Handlers;
using TBGServer.Packets;
using TBGServer.Tcp;

namespace TBGServer
{
    public class GameServer
    {
        #region properties
        private TcpListener _listener;

        // Clients objects
        private List<TcpClient> _clients;
        
        private HandlerManager _handlerManager;

        // Other data
        public readonly int Port;
        public bool Running { get; private set; }

        #endregion //properties


        public GameServer(int port)
        {
            Port = port;
            Running = false;
            _listener = new TcpListener(IPAddress.Any, Port);
            _clients = new List<TcpClient>();
            _handlerManager = new HandlerManager();
        }
        public void StartServer()
        {
            _ = Task.Run(() => RunServer());
        }

        public void Shutdown()
        {
            if (Running)
            {
                Running = false;
                Console.WriteLine("Shutting down the Game(s) Server...");
            }
        }

        private void RunServer()
        {
            Console.WriteLine("Starting the Game Server on port {0}", Port);
            Console.WriteLine("Press Ctrl-C to shutdown the server at any time.");

            _listener.Start();
            Running = true;
            List<Task> newConnectionTasks = new List<Task>();
            Console.WriteLine("Waiting for incommming connections...");

            while (Running)
            {
                if (_listener.Pending())
                    newConnectionTasks.Add(_handleNewConnection());
                foreach (TcpClient client in _clients.ToArray())
                {
                    EndPoint endPoint = client.Client.RemoteEndPoint;
                    bool disconnected = false;

                    JObject jObject = TcpReceiver.ReceivePacket(client).GetAwaiter().GetResult();
                    if (jObject != null)
                    {
                        _handlerManager.Handle(jObject, client);
                    }
                    disconnected = TcpHelper.IsDisconnected(client);

                    if (disconnected)
                    {
                        HandleDisconnectedClient(client);
                        Console.WriteLine("Client {0} has disconnected from the Game(s) Server.", endPoint);
                    }
                }
                Thread.Sleep(10);
            }

            Task.WaitAll(newConnectionTasks.ToArray(), 1000);

            Parallel.ForEach(_clients, (client) =>
            {
                HandleDisconnectedClient(client);
            });

            _listener.Stop();
            Console.WriteLine("The server has been shut down.");
        }
        private async Task _handleNewConnection()
        {
            TcpClient newClient = await _listener.AcceptTcpClientAsync();
            Console.WriteLine("New connection from {0}.", newClient.Client.RemoteEndPoint);
            _clients.Add(newClient);
            string msg = String.Format("Welcome to the \"{0}\" Games Server.\n", "localhost");
            await TcpSender.SendPacket(newClient, new RequestCredentialsPacket(msg));
        }
        public void HandleDisconnectedClient(TcpClient client)
        {
            _clients.Remove(client);
            TcpHelper.cleanupClient(client);
        }

    }
}
