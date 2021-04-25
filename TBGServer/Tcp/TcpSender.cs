using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TBGServer.Helpers;
using TBGServer.Packets;

namespace TBGServer.Tcp
{
    public class TcpSender
    {
        // Sends a packet to a client asynchronously
        public static async Task SendPacket(TcpClient client, Packet pac)
        {
            try
            {
                // convert JSON to buffer and its length to a 16 bit unsigned integer buffer
                byte[] jsonBuffer = Encoding.UTF8.GetBytes(JsonSerialization.Serialize(pac));
                byte[] lengthBuffer = BitConverter.GetBytes(Convert.ToUInt16(jsonBuffer.Length));

                // Join the buffers
                byte[] msgBuffer = new byte[lengthBuffer.Length + jsonBuffer.Length];
                lengthBuffer.CopyTo(msgBuffer, 0);
                jsonBuffer.CopyTo(msgBuffer, lengthBuffer.Length);

                // Send the packet
                await client.GetStream().WriteAsync(msgBuffer, 0, msgBuffer.Length);

                Console.WriteLine("[SENT]\n{0}", pac);
            }
            catch (Exception e)
            {
                // There was an issue is sending
                Console.WriteLine("There was an issue receiving a packet.");
                Console.WriteLine("Reason: {0}", e.Message);
            }
        }
    }
}