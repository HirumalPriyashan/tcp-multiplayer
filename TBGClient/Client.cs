using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TBGClient.Handlers;
using TBGClient.Packets;
using TBGClient.Tcp;

namespace TBGClient
{
    public class Client
    {
        #region Properties
        private static Client _instance = null;
        private static object _lock = new object();
        private readonly string _serverAddress;
        private readonly int _port;
        public bool _running { get; private set; }
        private TcpClient _client;
        private bool _clientRequestedDisconnect = false;
        private ConcurrentQueue<Packet> _outgoingMessages;
        private IHandler<JObject> _handler;
        #endregion
        
        #region Constructor
        private Client()
        {
            _client = new TcpClient();
            _outgoingMessages = new ConcurrentQueue<Packet>();
            _running = false;
            _serverAddress = "localhost";
            _port = 9000;
        }
        #endregion

        #region public methods

        /// <summary>
        /// Get Client instance
        /// </summary>
        /// <returns>Client Instance</returns>
        public static Client GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Client();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Connect client to the server and start accepting for incomming messages
        /// </summary>
        public void Connect()
        {
            try
            {
                _client.Connect(_serverAddress, _port);
            }
            catch (SocketException se)
            {
                Console.WriteLine("[ERROR] {0}", se.Message);
            }

            if (_client.Connected)
            {
                Console.WriteLine("Connected to the server at {0}.", _client.Client.RemoteEndPoint);
                _running = true;
                Task.Run(() => Run());
                Task.Run(() => SendMessagesInQueue());
            }
            else
            {
                TcpHelper.cleanupClient(_client);
                Console.WriteLine("Wasn't able to connect to the server at {0}:{1}.", _serverAddress, _port);
            }
        }

        /// <summary>
        /// Add message to the outgoing queue
        /// <strong>if client does not connected to server by now this method with connect<strong>
        /// </summary>
        /// <param name="packet">Packet which need to send to the server</param>
        public void SendMessage(Packet packet)
        {
            if (!_client.Connected)
            {
                Connect();
            }
            _outgoingMessages.Enqueue(packet);
        }

        /// <summary>
        /// Set handler for incoming messages
        /// </summary>
        /// <param name="handler">Handler to accept requests</param>
        public void SetHandler(IHandler<JObject> handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// Disconnect Client from server
        /// </summary>
        public void Disconnect()
        {
            Console.WriteLine("Disconnecting from the server...");
            _running = false;
            _clientRequestedDisconnect = true;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Run Client
        /// </summary>
        private void Run()
        {
            bool wasRunning = _running;

            List<Task> tasks = new List<Task>();
            while (_running)
            {
                tasks.Add(HandleIncomingPackets());
                Thread.Sleep(10);

                if (TcpHelper.IsDisconnected(_client) && !_clientRequestedDisconnect)
                {
                    _running = false;
                    Console.WriteLine("The server has disconnected from us ungracefully.\n:[");
                }
            }

            Task.WaitAll(tasks.ToArray(), 1000);

            TcpHelper.cleanupClient(_client);
            if (wasRunning)
                Console.WriteLine("Disconnected.");
        }
        /// <summary>
        /// Task to handle incomming messages from the server
        /// </summary>
        private async Task HandleIncomingPackets()
        {
            if (_client.Available > 0)
            {
                JObject jObject = await TcpReceiver.ReceivePacket(_client);
                _handler.Handle(jObject);
            }
        }
        /// <summary>
        /// Task to send messages in the outgoing queue to the server
        /// </summary>
        private async Task SendMessagesInQueue()
        {
            while (true)
            {
                if (_outgoingMessages.TryDequeue(out Packet result))
                {
                    await TcpSender.SendPacket(_client, result);
                }
            }
        }
        #endregion
    }
}