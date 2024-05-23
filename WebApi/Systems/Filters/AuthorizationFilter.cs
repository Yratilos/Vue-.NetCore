using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebApi.IUtils;
using WebApi.Systems.Extensions;
using WebApi.Systems.Results;
using WebApi.Utils;

namespace WebApi.Systems.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        IJwtUtil jwtService;
        IConfiguration Configuration;
        ILogger logger;
        public AuthorizationFilter(IJwtUtil jwtService, IConfiguration configuration, ILogger logger)
        {
            Configuration = configuration;
            this.jwtService = jwtService;
            this.logger = logger;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (Configuration["Jwt:Enabled"].ToBoolean() && !IsAllowAnonymous(context))
            {
                string token = context.HttpContext.Request.Headers["Authorization"];
                if (jwtService.IsValidToken(token, out string msg))
                {
                    var id = context.HttpContext.User.Claims.Single(c => c.Type == "ID").Value;
                    msg = "No access";
                    if (id != "08d3fa5a-9ae6-ee11-9c29-5a44875600c1")
                    {
                        ReturnStatusCode(context, msg, 403);
                    }
                }
                else
                {
                    ReturnStatusCode(context, msg, 401);
                }
            }
        }

        private void ReturnStatusCode(AuthorizationFilterContext context, string msg, int statusCode)
        {
            var rspResult = new CustomResponse<object>() { Status = CustomStatus.Error, Message = msg };
            context.Result = new InternalServerObjectResult(rspResult, statusCode);
            logger.Info(new LoggerDto()
            {
                RequestPath = context.HttpContext.Request.Path,
                RequestMethod = context.HttpContext.Request.Method,
                ClientIP = context.HttpContext.Connection.RemoteIpAddress,
                ClientPort = context.HttpContext.Connection.RemotePort,
                Result = msg,
                DataBase = Configuration["DataBase"]
            });
        }

        private bool IsAllowAnonymous(AuthorizationFilterContext context)
        {
            // 检查是否标记了 [AllowAnonymous] 特性
            return context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }

    }
}
