using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Networking.SocketServer
{
    public class SocketEventHandler<T> : SocketEventResponse where T : SocketResponse
    {
        public override void HandleResponse(JToken data, ResponseMeta meta) {
            MetaData = meta;
            var result = ParseData(data);
            OnSocketEvent.Invoke(result);
        }

        protected virtual T ParseData(JToken data) => JsonConvert.DeserializeObject<T>(data.ToString());

        public static Action<T> OnSocketEvent { get; set; } = delegate { };
    }
}