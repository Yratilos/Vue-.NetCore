using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.IServices;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class JobController : ControllerBase
    {
        IJobService jobService;
        public JobController(IJobService jobService)
        {
            this.jobService = jobService;
        }
        [HttpPost]
        public Job Log([FromBody] JobDto jobDto)
        {
            return jobService.Log(jobDto);
        }
        [HttpPost]
        public Job Share([FromBody] JobDto jobDto)
        {
            return jobService.Share(jobDto);
        }
    }
}
