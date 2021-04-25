using System;
using TBGClient.Packets;

namespace TBGClient
{
    public class Program
    {
        public static Client gamesClient;

        public static void InterruptHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            gamesClient?.Disconnect();
        }

        public static void Main()
        {
            Console.Title = "Client";
            gamesClient = Client.GetInstance();

            Console.CancelKeyPress += InterruptHandler;
            Console.WriteLine("Enter to connect");
            Console.ReadLine();
            gamesClient.Connect();
            Console.WriteLine("Enter to send CreateGamePacket");
            Console.ReadLine();
            gamesClient.SendMessage(new CreateGamePacket("gg", Enums.GameType.pub, "hi"));
            Console.WriteLine("Enter to send WrongCredPacket");
            Console.ReadLine();
            gamesClient.SendMessage(new CredentialsPacket("ass", "asdf"));
            Console.WriteLine("Enter to send RightCredPacket");
            Console.ReadLine();
            gamesClient.SendMessage(new CredentialsPacket("asdf", "asdf"));
            Console.WriteLine("Enter to send CreateGamePacket");
            Console.ReadLine();
            gamesClient.SendMessage(new CreateGamePacket("gg", Enums.GameType.pub, "Guid.().ToString()"));
            Console.WriteLine("Enter to send CreateGamePacket");
            Console.ReadLine();
            gamesClient.SendMessage(new CreateGamePacket("gg", Enums.GameType.pub, "Guid.NewGuid().ToString()"));
            Console.ReadLine();
        }
    }
}