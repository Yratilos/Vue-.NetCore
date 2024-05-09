using Microsoft.AspNetCore.Mvc;

namespace WebApi.Systems.Filters
{
    public class InternalServerObjectResult : ObjectResult
    {
        public InternalServerObjectResult(object value, int statusCode) : base(value)
        {
            StatusCode = statusCode;// StatusCodes
        }
    }
}
