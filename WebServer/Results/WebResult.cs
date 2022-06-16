using BestHTTP;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Networking.WebServer
{
    public class WebResult
    {
        public WebError Error;
        
        protected bool m_state;
        
        private HTTPResponse m_response;
        private bool m_needAuth;
        private string m_error;

        public void SetResponse(HTTPResponse response) {
            m_response = response;
            if (IsSuccessCallback) {
                HandleMeta(m_response);
            }
            else {//something bad happened in http protocol
                OnError(new WebError("Oops, internal error"));
            }
        }

        private void HandleMeta(HTTPResponse response) {
            Debug.LogError($"response.DataAsText: {response.DataAsText}");
            var node = JObject.Parse(response.DataAsText);
            if (node.TryGetValue("state", out var token)) {
                Session.SetSessionId(node.Value<string>("session_id"));
                m_state = node.Value<bool>("state");
                m_error = node.Value<string>("error");
            }

            if (!m_state) {//response has error token(localization will translate it depends on current language)
                OnError(new WebError(m_error));
            }
            else { 
                HandleResponse(m_response);
            }
        }

        private bool IsNeedAuthorize(JObject node) {
            if (node.TryGetValue("need_auth", out var token)) {
                m_needAuth = node.Value<bool>("error");
            }

            return m_needAuth;
        }

        protected virtual void HandleResponse(HTTPResponse response) {
            Debug.LogError($"response: {response.DataAsText}");
        }

        private bool IsSuccessCallback {
            get {
                if (m_response == null) {
                    return false;
                }

                return m_response.StatusCode == 200;
            }
        }

        private void OnError(WebError e) {
            Error = e;
        }

        public bool HasError => Error != null;

        public bool State => m_state;
    }
}