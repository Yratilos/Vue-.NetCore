using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.IUtils;
using WebApi.Systems.Results;

namespace WebApi.Systems.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        ILogger logger;

        public ExceptionFilter(ILogger logger)
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            //异常返回结果包装
            var rspResult = new CustomResponse<object>() { Status = CustomStatus.Error, Message = context.Exception.Message };
            logger.Error(context.Exception);
            context.ExceptionHandled = true;
            context.Result = new InternalServerObjectResult(rspResult, 500);
        }
    }
}
