using System.Collections.Generic;
using BestHTTP;
using UnityEngine;

namespace Networking.WebServer
{
    public abstract class WebRequest : IWebRequest
    {
        
        public string Mod{ get; }
        public string Act{ get;  }
        private string m_url;
        
        public virtual Dictionary<string, object> GenerateData() {
            var data = new Dictionary<string, object> {
                { "mod", Mod },
                { "act", Act }
            };
            if (Session.HasId()) {
                data.Add("session_id", Session.Id);
            }

            return data;
        }

        public abstract void OnRequestFinished(HTTPRequest originalRequest, HTTPResponse response);

        protected WebRequest(string mod, string act) {
            Mod = mod;
            Act = act;
            m_url = WebServer.Url;
            TimeStamp = WebServer.CurrentTimeStamp;
        }

        protected void Send() {
            WebServer.Send(this);
        }

        public string Url => m_url;

        public int TimeStamp { get; }

        public virtual HTTPMethods Method => HTTPMethods.Post;

        public bool HasTextureToSend => Texture != null;
        public Texture2D Texture { get; protected set;}
    }
}