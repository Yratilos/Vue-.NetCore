using System;
using System.Collections.Generic;
using WebApi.IRepositorys;
using WebApi.IServices;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User Add(User user)
        {
            var data=userRepository.Add(user);
            return data;
        }

        public User Delete(Guid id, out bool result)
        {
            var data = userRepository.Delete(id, out result);
            return data;
        }

        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public User GetById(Guid id)
        {
            return userRepository.GetById(id);
        }

        public string GetName(Guid id)
        {
            return userRepository.GetName(id);
        }
    }
}
