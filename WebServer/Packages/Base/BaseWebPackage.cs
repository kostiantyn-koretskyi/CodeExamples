using System;
using BestHTTP;
using UnityEngine;

namespace Networking.WebServer
{
    public abstract class BaseWebPackage<T> : WebRequest where T : WebResult, new()
    {
        private Action<T> m_callback = delegate { };
        private T m_result;

        protected BaseWebPackage(string mod, string act) : base(mod, act) { }

        public void Send(Action<T> callback) {
            m_callback += callback;
            base.Send();
        }

        public override void OnRequestFinished(HTTPRequest originalRequest, HTTPResponse response) {
            ShowLog(response);
            m_result = new T();
            m_result.SetResponse(response);
            m_callback(m_result);
        }

        private void ShowLog(HTTPResponse response) {
            if (response == null) {
                Debug.Log("WEB_IN::Data response is null " + typeof(T));
                return;
            }

            if (response.StatusCode == 200) {
                Debug.Log($"WEB_IN::Data {typeof(T)} response data: {response.DataAsText}");
            }
            else {
                var msg = $"Response error: {response.StatusCode} | Text: {response.Message}";
                Debug.Log(msg);
            }
        }
    }
}