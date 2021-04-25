using Newtonsoft.Json;
using TBGServer.Enums;

namespace TBGServer.Packets
{
    public abstract class Packet
    {
        [JsonProperty("type")]
        public MsgType Type { get; protected set; }
        [JsonProperty("msg")]
        public string Msg { get; protected set; }
        protected Packet(MsgType type, string msg)
        {
            this.Type = type;
            this.Msg = msg;
        }

        public override string ToString()
        {
            return string.Format(
                "[Packet:\n" +
                "  Type=`{0}`\n" +
                "  Message=`{1}`]",
                Type, Msg);
        }
    }
}
