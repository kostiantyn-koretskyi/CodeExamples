namespace Networking.WebServer
{
    public class WebError
    {

        public string Token { get; }

        public WebError(string token) {
            Token = token;
        }
    }
}