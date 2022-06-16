using System.Collections.Generic;

namespace Networking.SocketServer
{
    public class ResponseMeta
    {
        public long ServerPId { get; }
        public long ServerRelativeTime { get; }
        public string PacketType { get; }
        public List<string> ActionsList { get; }

        public ResponseMeta(long pid, long relativeTime, string packetType, List<string> actions)
        {
            ServerPId = pid;
            ServerRelativeTime = relativeTime;
            PacketType = packetType;
            ActionsList = actions;
        }
    }
}