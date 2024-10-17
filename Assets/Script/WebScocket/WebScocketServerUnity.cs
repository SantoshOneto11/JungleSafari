using WebSocketSharp;
using WebSocketSharp.Server;

namespace WepSocketUnity
{
    public class WebScocketServerUnity : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var message = e.Data == "BALUS"
                ? "Are you kidding?"
                : "I'm not available now.";

            Send(message);
        }
    }
}
