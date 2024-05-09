using System.ComponentModel;

namespace WebApi.Systems.Results
{
    public enum CustomStatus
    {
        [Description("请求失败")]
        Error = 0,
        [Description("请求成功")]
        Success = 1
    }
}