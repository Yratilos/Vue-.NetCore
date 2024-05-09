using System;

namespace WebApi.Systems.Results
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NoWrapperResultAttribute : Attribute
    {
    }
}
