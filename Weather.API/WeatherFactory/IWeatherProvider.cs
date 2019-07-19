using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.API.Models;

namespace Weather.API.WeatherFactory
{
    interface IWeatherProvider
    {
        Task<WeatherInfo> GetWeatherInfo(string cityId);

        Task<bool> GetBulkWeatherInfo();

        Task<bool> GetWeatherInfo(List<City> cities);
    }
}
