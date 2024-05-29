using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApi.IServices;
using WebApi.IUtils;
using WebApi.Models;
using WebApi.Systems.Extensions;
using WebApi.Systems.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class UserController : Page
    {
        IUserService userService;
        IJwtUtil jwt;
        public UserController(IUserService userService, IJwtUtil jwt)
        {
            this.userService = userService;
            this.jwt = jwt;
        }

        [HttpPost]
        public User Add([FromBody] User user)
        {
            var data = userService.Add(user);
            return data;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public CustomResponse<string> Token()
        {
            if(jwt.CreateToken(null, out string token))
            {
                return token;
            }
            return Error<string>(token);
        }

        // GET: api/<UserController>
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<User> Get()
        {
            var data = userService.GetAll();
            return data;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(Guid id)
        {
            var data = userService.GetById(id);
            return data;
        }

        // POST api/<UserController>
        [HttpPost]
        public User Post([FromBody] string value)
        {
            var data = userService.GetById(value.ToGuid());
            return data;
        }

        // PUT api/<UserController>/5
        [Obsolete]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [Obsolete]
        [HttpDelete("{id}")]
        public User Delete(Guid id)
        {
            var data = userService.Delete(id, out _);
            return data;
        }
    }
}
