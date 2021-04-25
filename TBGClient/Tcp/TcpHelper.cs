using System.Net.Sockets;

namespace TBGClient.Tcp
{
    class TcpHelper
    {
        // Checks if a client has disconnected ungracefully
        public static bool IsDisconnected(TcpClient client)
        {
            try
            {
                Socket s = client.Client;
                return s.Poll(10 * 1000, SelectMode.SelectRead) && (s.Available == 0);
            }
            catch (SocketException)
            {
                return true; // We got a socket error, assume it's disconnected
            }
        }

        // cleans up resources for a TcpClient and closes it
        public static void cleanupClient(TcpClient client)
        {
            client.GetStream().Close();     // Close network stream
            client.Close();                 // Close client
        }
    }
}