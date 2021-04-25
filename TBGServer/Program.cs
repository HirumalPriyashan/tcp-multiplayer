using System;
using Newtonsoft.Json.Linq;
using TBGServer.Handlers;

namespace TBGServer
{
    public class Program
    {
        public static GameServer gameServer;

        public static void InterruptHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            gameServer?.Shutdown();
        }

        public static void Main()
        {
            Console.Title = "Server";
            int port = 9000;
            
            Console.CancelKeyPress += InterruptHandler;

            gameServer = new GameServer(port);
            gameServer.StartServer();
            Console.ReadLine();
        }
    }
}