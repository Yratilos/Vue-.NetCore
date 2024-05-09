using System;
using System.Threading.Tasks;

namespace WebApi.IUtils
{
    public interface IHttpUtil
    {
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Task<string> Post(string url, object obj);

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Obsolete]
        public Task<string> Delete(string url);

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Obsolete]
        public Task<string> Put(string url, object obj);

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<string> Get(string url);

    }
}
