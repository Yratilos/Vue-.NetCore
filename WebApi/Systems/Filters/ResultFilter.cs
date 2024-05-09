using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using WebApi.Systems.Results;

namespace WebApi.Systems.Filters
{
    public class ResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionWrapper = controllerActionDescriptor?.MethodInfo.GetCustomAttributes(typeof(NoWrapperResultAttribute), false).FirstOrDefault();
            var controllerWrapper = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(NoWrapperResultAttribute), false).FirstOrDefault();
            //如果包含NoWrapperAttribute则说明不需要对返回结果进行包装，直接返回原始值
            if (actionWrapper != null || controllerWrapper != null)
            {
                return;
            }

            //根据实际需求进行具体实现
            var rspResult = new CustomResponse<object>();
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult?.Value is null)
                {
                    rspResult.Status = CustomStatus.Error;
                    rspResult.Message = "No data";
                    context.Result = new ObjectResult(rspResult);
                }
                else
                {
                    //如果返回结果已经是Response<T>类型的则不需要进行再次包装了
                    if (objectResult.DeclaredType.IsGenericType && objectResult.DeclaredType?.GetGenericTypeDefinition() == typeof(CustomResponse<>))
                    {
                        return;
                    }
                    rspResult.Data = objectResult.Value;
                    context.Result = new ObjectResult(rspResult);
                }
                return;
            }
        }
    }
}
