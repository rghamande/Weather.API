using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Weather.API.Models;
using Weather.API.WeatherFactory;

namespace Weather.API.ServiceProviders.OpenWeather
{
    public class OpenWeather : IWeatherProvider
    {
        async Task<WeatherInfo> IWeatherProvider.GetWeatherInfo(string cityId)
        {
            var OpenWeatherAPIEndpoint = ConfigurationManager.AppSettings["OpenWeatherAPIEndpoint"].ToString();
            var OpenWeatherAPIKey = ConfigurationManager.AppSettings["OpenWeatherAPIKey"].ToString();

            OpenWeatherAPIEndpoint = $"{OpenWeatherAPIEndpoint}?id={cityId}&AppId={OpenWeatherAPIKey}";
            using (HttpClient httpClient = new HttpClient())
            {
                var responseMessage = await httpClient.GetAsync(OpenWeatherAPIEndpoint);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var WeatherInfoString = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WeatherInfo>(WeatherInfoString);
                }
                else
                {
                    return new WeatherInfo{Id=0,Name="No record found for given City Id"};
                }

            }
        }

        private async Task<bool> GetWeatherInfoFromAPI(List<City> cities)
        {
            var OpenWeatherAPIEndpoint = ConfigurationManager.AppSettings["OpenWeatherAPIEndpoint"].ToString();
            var OpenWeatherAPIKey = ConfigurationManager.AppSettings["OpenWeatherAPIKey"].ToString();
            bool status = false;
            //Looping can be avoided using Bulk Data retrieval endPoint
            foreach (City city in cities)
            {
                try
                {
                    var CityPath = await GetPath(city);
                   var OpenWeatherAPIUrl = $"{OpenWeatherAPIEndpoint}?id={city.Id}&AppId={OpenWeatherAPIKey}";
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var responseMessage = await httpClient.GetAsync(OpenWeatherAPIUrl);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var WeatherInfoString = await responseMessage.Content.ReadAsStringAsync();
                           // WeatherInfo WeatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(WeatherInfoString);
                            status = await SaveResponseToFile(WeatherInfoString,$"{CityPath}\\{DateTime.Now.ToFileTime()}.json");
                        }
                        else
                        {
                            status = await SaveResponseToFile(responseMessage.ReasonPhrase, $"{CityPath}\\{DateTime.Now.ToFileTime()}.json");
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }

        private async Task<bool> SaveResponseToFile(string WeatherInfoString,string path)
        {
            try
            {
                File.AppendAllText(path, WeatherInfoString);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<String> GetPath(City city)
        {
            var rootPath= HttpContext.Current.Server.MapPath($"{ConfigurationManager.AppSettings.Get("APIResponsePath")}/{city.Id}/" +
                $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}");
            if (!Directory.Exists(rootPath))
            {
               Directory.CreateDirectory(rootPath);
            }
            return rootPath;
        }

        private async Task<List<City>> GetCities()
        {
            List<City> cities = new List<City>();
            using (StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("APICitiesPath"))))
            {
                string json = r.ReadToEnd();
                cities = JsonConvert.DeserializeObject<List<City>>(json);
            }
            return cities;
        }

        public async Task<bool> GetBulkWeatherInfo()
        {
            List<City> cities = await GetCities();
            bool processCompleted = await GetWeatherInfoFromAPI(cities);
            return processCompleted;
        }


        public async Task<bool> GetWeatherInfo(List<City> cities)
        {
                bool processCompleted = await GetWeatherInfoFromAPI(cities);
                return processCompleted;
        }
    }
}