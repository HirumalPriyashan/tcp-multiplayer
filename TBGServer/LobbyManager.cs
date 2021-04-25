namespace TBGServer
{
    public class LobbyManager
    {
        private static LobbyManager _instance = null;
        private static object _lock = new object();
        private LobbyManager() { }
        public static LobbyManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new LobbyManager();
                    }
                }
            }
            return _instance;
        }
    }
}