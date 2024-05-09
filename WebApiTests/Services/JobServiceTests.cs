using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Services;
using WebApi.IServices;
using WebApi.Utils;
using WebApi.IRepositorys;
using WebApiTests;
using WebApi.Models;
using WebApi.Dtos;

namespace WebApi.Services.Tests
{
    [TestClass()]
    public class JobServiceTests
    {
        static IJobRepository jobRepository
        {
            get
            {
                var db = Common.GetDataBase();
                var Configuration = Common.GetConfiguration();
                IJobRepository jobRepository;
                switch (Configuration["DataBase"])
                {
                    case "KingBase":
                        jobRepository = new Repositorys.KingBaseRepositorys.JobKingBase(db);
                        break;
                    default:
                        jobRepository = new Repositorys.SqlServerRepositorys.JobSqlServer(db);
                        break;
                }
                return jobRepository;
            }
        }
        static IJobService jobService
        {
            get
            {
                return new JobService(jobRepository);
            }
        }

        [DataRow("内容", "模块")]
        [TestMethod()]
        public void LogTest(string content, string model)
        {
            var j=jobService.Log(new JobDto() { Content = content, Model = model });
            var d = jobRepository.Delete(j);
            Assert.AreEqual(d.Content, content);
            Assert.AreEqual(d.Model, model);
        }

        [DataRow("内容", "模块")]
        [TestMethod()]
        public void ShareTest(string content,string model)
        {
            var j=jobService.Share(new JobDto() { Content= content,Model=model});
            var d = jobRepository.Delete(j);
            Assert.AreEqual(d.Content, content);
            Assert.AreEqual(d.Model, model);
        }
    }
}