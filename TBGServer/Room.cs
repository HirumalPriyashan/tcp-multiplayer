using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using TBGServer.Enums;
using TBGServer.Helpers;
using TBGServer.Packets;
using TBGServer.Players;
using TBGServer.Tcp;

namespace TBGServer
{
    public class Room
    {
        #region Properties
        // state of the game
        private ThreadSafe<GameState> _state = new ThreadSafe<GameState>();
        private string _gameId;
        private string _gameName;
        private GameType _gameType;
        // list of players
        private ClientPlayer _hostPlayer;
        private ConcurrentBag<ClientPlayer> _players = new ConcurrentBag<ClientPlayer>();
        // queue for incoming messages
        private ConcurrentQueue<Packet> _incomingMessages
            = new ConcurrentQueue<Packet>();

        // queue for outgoing messages
        private ConcurrentQueue<Tuple<Packet, ClientPlayer>> _outgoingMessages
            = new ConcurrentQueue<Tuple<Packet, ClientPlayer>>();

        // game thread
        private Thread _gameThread;
        private int _maxPlayers = 10;
        #endregion

        #region Public methods

        /// <summary>
        /// Create room with given name, max player count, type with unique id and returns unique id
        /// </summary>
        /// <param name="player" type="ClientPlayer">Used to identify who created the game</param>
        /// <param name="name">Name for the game instance, doesn't need to be unique</param>
        /// <param name="maxCount">Maximum number of players for the game</param>
        /// <param name="type">Type of the game ex:- public or private</param>
        /// <returns>unique identifier for the room as a string</returns>
        public string CreateRoom(ClientPlayer player, string name, GameType type)
        {
            _state.Value = GameState.WaitingForPlayers;
            _gameName = name;
            // TODO: generate unique game id and set it to id
            _gameId = Guid.NewGuid().ToString("N");
            _hostPlayer = player;
            _gameType = type;
            this.AddPlayer(player);
            Console.WriteLine(_gameId);
            _outgoingMessages.Enqueue(new Tuple<Packet, ClientPlayer>(new CreateGameSuccessPacket(_gameId), player));
            Task.Run(() => SendMessages());
            return _gameId;
        }

        /// <summary>
        /// Add given player to the game instance and update game state if reached max
        /// </summary>
        /// <param name="player">Client player who need to add to the game</param>
        /// <returns>whether player added successfully or not</returns>
        public bool AddPlayer(ClientPlayer player)
        {
            if (_state.Value == GameState.WaitingForPlayers)
            {
                _players.Add(player);
                if (_players.Count == _maxPlayers)
                {
                    _state.Value = GameState.WaitingForGameStart;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// get game id for game instance
        /// </summary>
        /// <returns>id for asked game</returns>
        public string GetRoomId()
        {
            return _gameId;
        }

        public GameState GetGameState()
        {
            return _state.Value;
        }

        /// <summary>
        /// remove given player from the game instance and update game state 
        /// <strong> TODO: this method is not working properly</strong>
        /// </summary>
        /// <param name="player">Client player who need to remove from the game</param>
        /// <returns>whether player removed successfully or not</returns>
        public bool RemovePlayer(ClientPlayer player)
        {
            if (_state.Value == GameState.WaitingForPlayers || _state.Value == GameState.WaitingForGameStart)
            {
                // _players.TryTake(player);
                _state.Value = GameState.WaitingForPlayers;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Start the game
        /// </summary>
        public void StartGame(ClientPlayer client)
        {
            if (_state.Value == GameState.WaitingForGameStart && client == _hostPlayer)
            {
                _gameThread = new Thread(new ThreadStart(Run));
                _gameThread.Start();
                _state.Value = GameState.InGame;
            }
        }

        /// <summary>
        /// Enque incoming packet to the incomming packet queue
        /// </summary>
        /// <param name="packet">Command as a packet</param>
        public void PassMessage(Packet packet)
        {
            _incomingMessages.Enqueue(packet);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Task to send messages in the outgoing queue to clients
        /// </summary>
        private async Task SendMessages()
        {
            while (_state.Value != GameState.GameOver)
            {
                if (_outgoingMessages.TryDequeue(out Tuple<Packet, ClientPlayer> result))
                {
                    await TcpSender.SendPacket(result.Item2.TcpClient, result.Item1);
                }
            }
        }

        /// <summary>
        /// Run game loop
        /// </summary>
        private void Run()
        {
            throw new NotImplementedException();
        }

        public void StopGame()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
