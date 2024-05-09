using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.IUtils;

namespace WebApi.Utils
{

    public class WebSocketUtil : IWebSocketUtil
    {
        Dictionary<WebSocket, List<WebSocketDto>> _webSockets = new Dictionary<WebSocket, List<WebSocketDto>>();
        ILogger logger;

        public WebSocketUtil(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task SendMessageToAllAsync(string message)
        {
            var tasks = new List<Task>();
            foreach (var webSocket in _webSockets)
            {
                if (webSocket.Key.State == WebSocketState.Open)
                {
                    tasks.Add(webSocket.Key.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None));
                }
            }
            await Task.WhenAll(tasks);
        }

        public void AddWebSocket(WebSocket webSocket)
        {
            _webSockets.Add(webSocket, new List<WebSocketDto>());
        }

        public void RemoveWebSocket(WebSocket webSocket)
        {
            _webSockets.Remove(webSocket);
        }

        public void AddMessage(WebSocket webSocket, string msg, IPAddress clientIP, int clientPort)
        {
            var lst = _webSockets[webSocket];
            var webSocketDto = new WebSocketDto { Message = msg, ClientIP = clientIP, ClientPort = clientPort };
            lst.Add(webSocketDto);
            logger.WebSocket(webSocketDto);
            _webSockets[webSocket] = lst;
        }

        public Dictionary<WebSocket, List<WebSocketDto>> GetMessages()
        {
            return _webSockets;
        }

        public async Task SendMessageToAsync(Dictionary<WebSocket, List<WebSocketDto>> webSockets, string message)
        {
            var tasks = new List<Task>();
            foreach (var webSocket in webSockets)
            {
                if (webSocket.Key.State == WebSocketState.Open)
                {
                    tasks.Add(webSocket.Key.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None));
                }
            }
            await Task.WhenAll(tasks);
        }
    }

}
