using System.Security.Claims;

namespace WebApi.IUtils
{
    public interface IJwtUtil
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="claims">
        /// Claim[] claims = new Claim[]{
        ///     new Claim(key,value)//可以存用户信息不敏感数据
        /// }
        /// </param>
        /// <returns></returns>
        public string CreateToken(Claim[] claims);
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsValidToken(string token, out string message);
    }
}
