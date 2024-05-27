using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApi.Dtos;
using WebApi.IRepositorys;
using WebApi.IServices;
using WebApi.Models;

namespace WebApi.Services
{
    public class JobService : IJobService
    {
        IJobRepository jobRepository;
        public JobService(IJobRepository jobRepository)
        {
            this.jobRepository = jobRepository;
        }

        public List<Job> GetAll()
        {
            return jobRepository.GetAll();
        }

        public Job Log(JobDto j)
        {
            return Add(j, jobLogType.Log);
        }

        public Job Share(JobDto j)
        {
            return Add(j, jobLogType.Share);
        }

        Job Add(JobDto j, jobLogType type)
        {
            var job = jobRepository.Add(new Job() { ID = Guid.NewGuid(), LogType = type.ToString(), Content = j.Content, Model = j.Model });
            var lst = jobRepository.GetAll().OrderBy(s => s.CreateTime).ToList();
            var json = JsonConvert.SerializeObject(lst);
            StreamWriter writer = new StreamWriter("Job.json");
            writer.Write(json);
            writer.Close();
            return job;
        }

    }
}
