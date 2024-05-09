using System;
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.IServices
{
    public interface IUserService
    {
        User GetById(Guid id);
        User Delete(Guid id, out bool result);
        List<User> GetAll();
        string GetName(Guid id);
        User Add(User user);
    }
}
