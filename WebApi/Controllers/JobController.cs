using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.IServices;

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
        public JobDto Log([FromBody] JobDto jobDto)
        {
            jobService.Log(jobDto);
            return jobDto;
        }
        [HttpPost]
        public JobDto Share([FromBody] JobDto jobDto)
        {
            jobService.Share(jobDto);
            return jobDto;
        }
    }
}
