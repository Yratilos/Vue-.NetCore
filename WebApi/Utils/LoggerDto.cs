using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using WebApi.Systems.Extensions;
using WebApi.Systems.Results;

namespace WebApi.Utils
{
    public class LoggerDto
    {
        public LoggerDto()
        {
            CreateTime = DateTime.Now;
        }
        public IPAddress ClientIP { get; set; }
        public int ClientPort { get; set; }
        public DateTime CreateTime { get; set; }
        public string RequestPath { get; set; }
        public IDictionary<string, object> RequestParams { get; set; }
        public TimeSpan Time { get; set; }
        public string RequestMethod { get; set; }
        object _result;
        public object Result { get { return _result is null ? CustomStatus.Error.GetDescription() : _result; } set { Result = value; } }
        public string DataBase { get; set; }
        public override string ToString()
        {
            return $"Request Method: {RequestMethod}, Request Path: {RequestPath}, Parameters: {JsonConvert.SerializeObject(RequestParams)}, Result: {JsonConvert.SerializeObject(Result)}, Time taken: {Time.TotalMilliseconds} ms, DataBase:{DataBase}, IP: {ClientIP}, Port: {ClientPort}";
        }
    }
}
