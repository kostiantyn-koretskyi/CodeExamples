using System.Collections.Generic;
using BestHTTP;
using UnityEngine;

namespace Networking.WebServer
{
    public interface IWebRequest
    {
        string Url { get; }
        HTTPMethods Method { get; }
        bool HasTextureToSend { get; }
        Texture2D Texture { get; }
        Dictionary<string, object> GenerateData();
        void OnRequestFinished(HTTPRequest originalRequest, HTTPResponse response);
        string Act{ get;}
        string Mod{ get; }
    }
}