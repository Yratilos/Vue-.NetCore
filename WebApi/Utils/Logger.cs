using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using WebApi.IUtils;
using WebApi.Systems.Extensions;

namespace WebApi.Utils
{
    public class Logger : ILogger
    {
        private static readonly string LogWebSocketPath = "LogWebSocket.txt";
        private static readonly string LogInfoPath = "LogInfo.txt";
        private static readonly string LogErrorPath = "LogError.txt";
        private static readonly string LogJobPath = "LogJob.txt";
        private static readonly string LogSharePath = "LogShare.txt";

        IConfiguration Configuration;
        public Logger(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        void Write(string message, string path)
        {
            if (!Configuration["Logger"].ToBoolean())
            {
                return;
            }
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
        }

        [Obsolete]
        public void Job(string message)
        {
            Write(message, LogJobPath);
        }

        public void WebSocket<T>(T model)
        {
            Write(model.ToString(), LogWebSocketPath);
        }

        public void Info<T>(T model)
        {
            Write(model.ToString(), LogInfoPath);
        }

        public void Error(Exception message)
        {
            Write(message.ToString(), LogErrorPath);
        }

        [Obsolete]
        public void Share(string message)
        {
            Write(message,LogSharePath);
        }
    }
}
