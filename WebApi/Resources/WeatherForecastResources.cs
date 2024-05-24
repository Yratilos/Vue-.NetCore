using Microsoft.Extensions.Localization;
using WebApi.Controllers;

namespace WebApi.Resources
{
    public class WeatherForecastResources
    {
        IStringLocalizer<WeatherForecastController> _localizer;
        public WeatherForecastResources(IStringLocalizer<WeatherForecastController> localizer)
        {
            _localizer = localizer;
        }
        /// <summary>
        /// 温和
        /// </summary>
        public string Freezing { get { return _localizer["Freezing"]; } }
        /// <summary>
        /// 凉爽
        /// </summary>
        public string Bracing { get { return _localizer["Bracing"]; } }
        /// <summary>
        /// 寒冷
        /// </summary>
        public string Chilly { get { return _localizer["Chilly"]; } }
        /// <summary>
        /// 凉爽
        /// </summary>
        public string Cool { get { return _localizer["Cool"]; } }
        /// <summary>
        /// 极冷
        /// </summary>
        public string Mild { get { return _localizer["Mild"]; } }
        /// <summary>
        /// 热
        /// </summary>
        public string Warm { get { return _localizer["Warm"]; } }
        /// <summary>
        /// 温暖
        /// </summary>
        public string Balmy { get { return _localizer["Balmy"]; } }
        /// <summary>
        /// 炎热
        /// </summary>
        public string Hot { get { return _localizer["Hot"]; } }
        /// <summary>
        /// 闷热
        /// </summary>
        public string Sweltering { get { return _localizer["Sweltering"]; } }
        /// <summary>
        /// 暖和
        /// </summary>
        public string Scorching { get { return _localizer["Scorching"]; } }
    }
}
