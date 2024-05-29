using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.IUtils;
using WebApi.Systems.Extensions;

namespace WebApi.Utils
{
    public class JwtUtil : IJwtUtil
    {
        IConfiguration Configuration;
        public JwtUtil(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool CreateToken(Claim[] claims,out string token)
        {
            // 1. 定义需要使用到的Claims
            if (claims is null)
            {
                claims = new Claim[]
                {
                    new Claim("ID", "08d3fa5a-9ae6-ee11-9c29-5a44875600c1"),
                    new Claim("Name", "test")
                };
            }

            // 2. 从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            // 3. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                Configuration["Jwt:Issuer"],     //Issuer
                Configuration["Jwt:Audience"],   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddSeconds(Configuration["Jwt:Timeout"].ToDouble()),    //expires
                signingCredentials               //Credentials
            );

            // 6. 将token变为string
            try
            {
                token = "Bearer " + new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return true;
            }
            catch (Exception)
            {
                token = "Invalid key";
                return false;
            }
        }

        public bool IsValidToken(string token, out string message)
        {
            if (string.IsNullOrEmpty(token))
            {
                message = "Token not found";
                return false;
            }
            token = token.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                message = "Token validation succeeded";
                return true; // 验证成功
            }
            catch (SecurityTokenException)
            {
                // 验证失败，SecurityTokenException 表示令牌验证失败
                message = "Token validation failed";
                return false;
            }
            catch (Exception)
            {
                // 其他异常，可能是令牌格式不正确等
                message = "The token format is incorrect";
                return false;
            }
        }

    }
}
