using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApi.Systems.Extensions
{
    public class CustomSession
    {
        HttpContext httpContext;
        public CustomSession(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext.HttpContext;
        }
        public object this[string key]
        {
            get
            {
                return httpContext.Session.GetString(key);
            }
            set
            {
                httpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
            }
        }
    }
}
