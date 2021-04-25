using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TBGServer.Enums;
using TBGServer.Packets;
using TBGServer.Players;
using TBGServer.Tcp;

namespace TBGServer
{
    public class RoomManager
    {
        private static RoomManager _instance = null;
        private static object _lock = new object();
        private LobbyManager _lobbyManager;
        private ConcurrentDictionary<string, Room> _games;
        // queue for outgoing messages
        private ConcurrentQueue<Tuple<Packet, ClientPlayer>> _outgoingMessages
            = new ConcurrentQueue<Tuple<Packet, ClientPlayer>>();
        private RoomManager()
        {
            _lobbyManager = LobbyManager.GetInstance();
            _games = new ConcurrentDictionary<string, Room>();
            Task.Run(() => RemoveFinishedGames());
            Task.Run(() => SendMessages());
        }
        public static RoomManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RoomManager();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Create game with given name, max player count, type with unique id
        /// </summary>
        /// <param name="player" type="ClientPlayer">Used to identify who created the game</param>
        /// <param name="name">Name for the game instance, doesn't need to be unique</param>
        /// <param name="maxCount">Maximum number of players for the game</param>
        /// <param name="type">Type of the game ex:- public or private</param>
        public void CreateGame(ClientPlayer player, string name, GameType type = GameType.pub)
        {
            Room game = new Room();
            string gameid = game.CreateRoom(player, name, type);
            _games.TryAdd(gameid, game);
        }

        /// <summary>
        /// Add given player to a given game instance specified by id
        /// </summary>
        /// <param name="player">Client player who need to add to the game</param>
        /// <param name="id">game id</param>
        public void AddPlayerToGame(ClientPlayer player, string id)
        {
            Room game = GetGameById(id);
            bool added = game.AddPlayer(player);
            if (!added)
            {
                throw new Exception("Player did not added");
            }
        }

        /// <summary>
        /// Add given packet to a given game instance specified by id
        /// </summary>
        /// <param name="packet">Packet to pass</param>
        /// <param name="id">game id</param>
        public void AddMessageToGame(Packet packet, string id)
        {
            Room game = GetGameById(id);
            game.PassMessage(packet);
        }

        /// <summary>
        /// Start a given game instance specified by id
        /// </summary>
        /// <param name="id">game id</param>
        public void StartGame(string id,ClientPlayer client)
        {
            Room game = GetGameById(id);
            game.StartGame(client);
        }

        /// <summary>
        /// Return Game Count
        /// </summary>
        /// <returns> Number of running games<returns>
        public void GameCount()
        {
            int count = _games.Count;
        }

        /// <summary>
        /// get List of Running games for a given client
        /// </summary>
        /// <returns> get running games<returns>
        public List<Room> GetGames()
        {
            List<Room> games = new List<Room>();
            foreach (KeyValuePair<string, Room> entry in _games)
            {
                games.Add(entry.Value);
            }
            return games;
        }
        /// <summary>
        /// Returns a given game instance specified by id
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>Game id <returns>
        private Room GetGameById(string id)
        {
            Room game = null;
            _games.TryGetValue(id, out game);
            if (game != null)
            {
                return game;
            }
            else
            {
                throw new Exception("Game is not in the game list");
            }
        }

        private Task RemoveFinishedGames()
        {
            while (true)
            {
                foreach (KeyValuePair<string, Room> entry in _games)
                {
                    if (entry.Value.GetGameState() == GameState.GameOver)
                    {
                        _games.Remove(entry.Key, out Room g);
                    }
                }
            }
        }

        private async Task SendMessages()
        {
            while (true)
            {
                if (_outgoingMessages.TryDequeue(out Tuple<Packet, ClientPlayer> result))
                {
                    await TcpSender.SendPacket(result.Item2.TcpClient, result.Item1);
                }
            }
        }
    }
}