using System;
using System.Net;

namespace WebApi.Utils
{
    public class WebSocketDto
    {
        public WebSocketDto()
        {
            CreateTime = DateTime.Now;
        }
        public IPAddress ClientIP { get; set; }
        public int ClientPort { get; set; }
        public DateTime CreateTime { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return $"Message: {Message}, IP: {ClientIP}, Port: {ClientPort}";
        }
    }
}
