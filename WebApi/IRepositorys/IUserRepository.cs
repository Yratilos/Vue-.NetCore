using System;
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.IRepositorys
{
    public interface IUserRepository
    {
        User GetById(Guid id);
        User Delete(Guid id, out bool result);
        List<User> GetAll();

        string GetName(Guid id);
        User Add(User user);
    }
}
