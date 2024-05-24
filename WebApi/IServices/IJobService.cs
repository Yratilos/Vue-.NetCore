using System.Collections.Generic;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.IServices
{
    public interface IJobService
    {
        public Job Log(JobDto j);
        public Job Share(JobDto j);
        public List<Job> GetAll();
    }
}
