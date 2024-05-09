using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.IUtils;

namespace WebApi.Utils
{
    public class WebSocketUtilMiddleware
    {
        private RequestDelegate _next;
        private IWebSocketUtil _webSocketService;
        ILogger _logger;

        public WebSocketUtilMiddleware(RequestDelegate next, IWebSocketUtil webSocketService, ILogger logger)
        {
            _next = next;
            _webSocketService = webSocketService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                _webSocketService.AddWebSocket(webSocket);
                var clientIP = context.Connection.RemoteIpAddress;
                var clientPort = context.Connection.RemotePort;


                try
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1024]);
                        WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
                        if (receiveResult.Count > 0 && receiveResult.MessageType == WebSocketMessageType.Text)
                        {
                            string message = Encoding.UTF8.GetString(receiveBuffer.Array, 0, receiveResult.Count);
                            _webSocketService.AddMessage(webSocket, message, clientIP, clientPort);
                        }
                    }
                    _webSocketService.RemoveWebSocket(webSocket);
                    webSocket.Dispose();
                }
                catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                {
                    _logger.Error(ex);
                    // 处理连接异常
                    _webSocketService.RemoveWebSocket(webSocket);
                }
            }
            else
            {
                await _next(context);
            }
        }
        /// <summary>
        /// 定期发送心跳消息并接受消息
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        [Obsolete]
        private async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                await Task.Delay(TimeSpan.FromSeconds(30)); // 30秒发送一次心跳消息
                // 定期发送心跳消息
                ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Keep alive"));
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

                // 接收客户端消息
                ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);

                // 判断消息类型
                if (receiveResult.Count > 0 && receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(receiveBuffer.Array, 0, receiveResult.Count);
                    // 处理接收到的消息
                    if (message != "Keep alive")
                    {
                        // 客户端发送了特定消息，可以根据需要进行处理
                        Trace.WriteLine(message);
                    }
                }
            }
        }
    }
}
