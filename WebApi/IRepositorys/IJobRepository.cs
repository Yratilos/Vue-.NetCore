using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.IRepositorys
{
    public interface IJobRepository
    {
        Job Add(Job job);
        Job Delete(Job id);
        List<Job> GetAll();
    }
}
