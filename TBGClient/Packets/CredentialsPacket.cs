using Newtonsoft.Json;
using TBGClient.Enums;

namespace TBGClient.Packets
{
    internal class CredentialsPacket : Packet
    {
        [JsonProperty("username")]
        public string username{ get; private set; }
        [JsonProperty("password")]
        public string password{ get; private set; }
        public CredentialsPacket(string username,string password, string msg = "") : base(MsgType.Credentials, msg)
        {
            this.username = username;
            // TODO: encrypt password
            this.password = password;
        }
    }
}