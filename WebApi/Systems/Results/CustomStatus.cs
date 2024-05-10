using System.ComponentModel;

namespace WebApi.Systems.Results
{
    public enum CustomStatus
    {
        /// <summary>
        ///  0 表示成功，好处在于只需要一个 code 表示成功，而其它非 0 的所有 code 可以用来表示不同类型的失败。
        /// 缺点在于作为 if 判断的时候不能直接把它当布尔量使用，需要取非一次，在逻辑上才成立。
        /// </summary>
        [Description("请求成功")]
        Success = 0,
        [Description("请求失败")]
        Error = 1
    }
}