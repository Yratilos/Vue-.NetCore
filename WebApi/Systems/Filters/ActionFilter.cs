using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using WebApi.IUtils;
using WebApi.Systems.Results;
using WebApi.Utils;

namespace WebApi.Systems.Filters
{
    public class ActionFilter : IActionFilter
    {
        private Stopwatch stopwatch;
        LoggerDto loggerDto;
        ILogger logger;
        IConfiguration Configuration;
        public ActionFilter(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger;
            Configuration = configuration;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch.Stop();
            loggerDto.Time = stopwatch.Elapsed;
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                var result = objectResult?.Value;
                if (objectResult.DeclaredType.IsGenericType && objectResult.DeclaredType?.GetGenericTypeDefinition() == typeof(CustomResponse<>))
                {
                    JObject obj = JObject.FromObject(result);
                    result = obj["Data"];
                }
                loggerDto.Result = result;
            }
            logger.Info(loggerDto);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch = Stopwatch.StartNew();
            loggerDto = new LoggerDto()
            {
                RequestParams = context.ActionArguments,
                RequestPath = context.HttpContext.Request.Path,
                RequestMethod = context.HttpContext.Request.Method,
                ClientIP = context.HttpContext.Connection.RemoteIpAddress,
                ClientPort = context.HttpContext.Connection.RemotePort,
                DataBase = Configuration["DataBase"]
            };
        }
    }
}
