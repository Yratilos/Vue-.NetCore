using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebApi.Utils;

namespace WebApi.IUtils
{
    public interface IWebSocketUtil
    {
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="msg"></param>
        public void AddMessage(WebSocket webSocket, string msg, IPAddress clientIP, int clientPort);
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        public Dictionary<WebSocket, List<WebSocketDto>> GetMessages();
        /// <summary>
        /// 给所有WebSocket发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageToAllAsync(string message);
        /// <summary>
        /// 给WebSocket发送消息
        /// </summary>
        /// <param name="webSockets"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageToAsync(Dictionary<WebSocket, List<WebSocketDto>> webSockets, string message);
        /// <summary>
        /// 添加WebSocket
        /// </summary>
        /// <param name="webSocket"></param>
        public void AddWebSocket(WebSocket webSocket);
        /// <summary>
        /// 删除WebSocket
        /// </summary>
        /// <param name="webSocket"></param>
        public void RemoveWebSocket(WebSocket webSocket);
    }
}
