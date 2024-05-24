using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Dtos;
using WebApi.Resources;

namespace WebApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        string[] Summaries
        {
            get
            {
                return new[]
                {
                    resources.Freezing, resources.Bracing, resources.Chilly, resources.Cool, resources.Mild,resources.Warm, resources.Balmy, resources.Hot, resources.Sweltering, resources.Scorching
                };
            }
        }
        WeatherForecastResources resources;

        public WeatherForecastController(IStringLocalizer<WeatherForecastController> localizer)
        {
            resources = new WeatherForecastResources(localizer);
        }

        [HttpGet]
        public IEnumerable<WeatherForecastDto> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
