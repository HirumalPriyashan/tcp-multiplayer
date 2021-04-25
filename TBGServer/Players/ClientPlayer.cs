using System.Net.Sockets;

namespace TBGServer.Players
{
    public class ClientPlayer
    {
        private TcpClient _tcpClient;
        private string _token;

        public TcpClient TcpClient { get => _tcpClient; private set => _tcpClient = value; }
        public string Token { get => _token; set => _token = value; }

        public ClientPlayer( string token, TcpClient client)
        {
            TcpClient = client;
            Token = token;
        }
    }
}