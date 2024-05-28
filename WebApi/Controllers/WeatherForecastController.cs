using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Dtos;
using WebApi.Resources;
using WebApi.Systems.Extensions;

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
        CustomCache cache; CustomSession session; Redis redis;

        public WeatherForecastController(IStringLocalizer<WeatherForecastController> localizer, CustomCache cache, CustomSession session, Redis redis)
        {
            resources = new WeatherForecastResources(localizer);
            this.cache = cache;
            this.session = session;
            this.redis = redis;
        }

        [HttpGet]
        public IEnumerable<WeatherForecastDto> Get()
        {
            cache["key"] = 1;
            var n = cache["key"];
            session["key"] = 2;
            var m = session["key"];
            redis["key"] = 3;
            var q = redis["key"];
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
