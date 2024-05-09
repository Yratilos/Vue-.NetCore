using WebApi.Systems.Extensions;

namespace WebApi.Systems.Results
{
    public class CustomResponse<T>
    {
        /// <summary>
        /// 状态结果
        /// </summary>
        public CustomStatus Status { get; set; } = CustomStatus.Success;

        private string _msg;

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Message
        {
            get
            {
                // 如果没有自定义的结果描述，则可以获取当前状态的描述
                return _msg is null ? Status.GetDescription() : _msg;
            }
            set
            {
                _msg = value;
            }
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 隐式将T转化为Response
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator CustomResponse<T>(T value)
        {
            if (value is null)
            {
                return null;
            }
            return new CustomResponse<T> { Data = value };
        }
    }
}
