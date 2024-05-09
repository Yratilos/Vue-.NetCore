using Microsoft.AspNetCore.Mvc;
using System;
using WebApi.Systems.Results;

namespace WebApi
{
    public abstract class Page : ControllerBase
    {
        /// <summary>
        /// 成功状态返回结果
        /// </summary>
        /// <param name="result">返回的数据</param>
        /// <returns></returns>
        [Obsolete]
        public CustomResponse<T> Success<T>(T result)
        {
            return new CustomResponse<T>() { Data = result };
        }

        /// <summary>
        /// 失败状态返回结果
        /// </summary>
        /// <param name="msg">失败信息</param>
        /// <returns></returns>
        public CustomResponse<T> Error<T>(string msg = null)
        {
            return new CustomResponse<T>() { Status = CustomStatus.Error, Message = msg };
        }

        /// <summary>
        /// 自定义状态返回结果
        /// </summary>
        /// <param name="status"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public CustomResponse<T> Result<T>(CustomStatus status, T result, string msg = null)
        {
            return new CustomResponse<T>() { Status = status, Data = result, Message = msg };
        }
    }
}
