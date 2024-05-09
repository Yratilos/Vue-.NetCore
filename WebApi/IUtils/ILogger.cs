using System;

namespace WebApi.IUtils
{
    public interface ILogger
    {
        /// <summary>
        /// 记录WebSocket接受的消息
        /// 对应类需要重写ToString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void WebSocket<T>(T model);
        /// <summary>
        /// 对应类需要重写ToString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void Info<T>(T model);
        public void Error(Exception message);
        public void Job(string message);
        public void Share(string message);
    }
}
