using Newtonsoft.Json.Linq;

namespace Networking.SocketServer
{
    public class SocketEventResponse : SocketResponse {
        public ResponseMeta MetaData;
        public virtual void HandleResponse(JToken data, ResponseMeta meta) {}
    }
}