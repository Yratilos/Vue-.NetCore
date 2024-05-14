using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Dtos;

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
                    _localizer["Freezing"].ToString(), _localizer["Bracing"], _localizer["Chilly"], _localizer["Cool"], _localizer["Mild"],_localizer["Warm"], _localizer["Balmy"], _localizer["Hot"], _localizer["Sweltering"], _localizer["Scorching"]
                };
            } 
        } 
        IStringLocalizer<WeatherForecastController> _localizer;

        public WeatherForecastController(IStringLocalizer<WeatherForecastController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
