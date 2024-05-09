using System;
using System.IO;
using WebApi.IUtils;

namespace WebApi.Utils
{
    public class Logger : ILogger
    {
        private static readonly string LogWebSocketPath = "LogWebSocket.txt";
        private static readonly string LogInfoPath = "LogInfo.txt";
        private static readonly string LogErrorPath = "LogError.txt";
        private static readonly string LogJobPath = "LogJob.txt";
        private static readonly string LogSharePath = "LogShare.txt";

        [Obsolete]
        public void Job(string message)
        {
            using (StreamWriter writer = File.AppendText(LogJobPath))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
        }

        public void WebSocket<T>(T model)
        {
            using (StreamWriter writer = File.AppendText(LogWebSocketPath))
            {
                writer.WriteLine(model.ToString());
            }
        }

        public void Info<T>(T model)
        {
            using (StreamWriter writer = File.AppendText(LogInfoPath))
            {
                writer.WriteLine(model.ToString());
            }
        }

        public void Error(Exception message)
        {
            using (StreamWriter writer = File.AppendText(LogErrorPath))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
        }

        [Obsolete]
        public void Share(string message)
        {
            using (StreamWriter writer = File.AppendText(LogSharePath))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
        }
    }
}
