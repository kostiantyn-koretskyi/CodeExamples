using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Networking.SocketServer {
    
    public abstract class AwaitableDeniableEvent<T> : SocketEventHandler<AwaitableDeniableEvent<T>>, IAwaitableEvent where T: DeniableData {

        public static Action<T, string> Action { get; set; }

        public abstract string ObjectId { get; protected set; }
        public abstract List<T> Data { get; protected set; }
        
        protected override AwaitableDeniableEvent<T> ParseData(JToken data) {
            ObjectId = data.Value<string>("object_id");
            Data = data["data"].ToObject<List<T>>();
            
            var id = ObjectId;
            foreach (var dat in Data) {
                dat.Action += () =>  Action(dat, id);
            }
            return this;
        }

    }

    public abstract class DeniableData {
        [JsonProperty("tr")] private long TR;
        public long RelativeTime => TR;
        public Action Action ;

        public abstract bool IsSimilar(DeniableData data);

        protected bool IsTimeSimilar(long time) {
            return Mathf.Abs(time - RelativeTime) < 10;
        }
    }
}