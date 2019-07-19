using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Weather.API.Models;
using Weather.API.WeatherFactory;

namespace Weather.API.Controllers
{
    public class WeatherInfoController : ApiController
    {
        private string _WeatherInfoProvider = ConfigurationManager.AppSettings["WeatherProvider"].ToString();


        /// <summary>
        // Gets the Weather Information for given cityId
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>WheatheInfo</returns>
        public async Task<WeatherInfo> Get(string cityId)
        {
            var WeatherProvider = new WeatherProvider();
            IWeatherProvider Weather = WeatherProvider.FactoryMethod(_WeatherInfoProvider);
            WeatherInfo WeatherInfo = await Weather.GetWeatherInfo(cityId);
            return WeatherInfo;
        }
       

        /// <summary>
        /// Gets the Weather Information for all the cities from master city.json file
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAll()
        {
            var WeatherProvider = new WeatherProvider();
          IWeatherProvider  Weather= WeatherProvider.FactoryMethod(_WeatherInfoProvider);
            bool success =await Weather.GetBulkWeatherInfo();
            if (success)
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            else
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Gets the Weather Information for list of cities from the file uploaded by the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetMany()
        {
            HttpResponseMessage result = null;
            var httpRequest = System.Web.HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                bool success = false;
                var postedFile = httpRequest.Files[0];
                var finfo = new FileInfo(postedFile.FileName);
                if (finfo.Extension.ToLower() == ".txt")
                {
                    var fname= $"{Path.GetFileNameWithoutExtension(finfo.FullName)}_{DateTime.Now.ToFileTime()}{finfo.Extension}";
                    var filePath = System.Web.HttpContext.Current.Server.MapPath("~/UserUploads/" + fname);
                    postedFile.SaveAs(filePath);
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();
                        var WeatherProvider = new WeatherProvider();
                        IWeatherProvider Weather = WeatherProvider.FactoryMethod(_WeatherInfoProvider);
                        try
                        {
                            var cities = JsonConvert.DeserializeObject<List<City>>(json);
                            success = await Weather.GetWeatherInfo(cities);
                        }
                        catch (Exception ex)
                        {
                            result = Request.CreateResponse(HttpStatusCode.BadRequest);
                        }
                    }
                }
                else { result = Request.CreateResponse(HttpStatusCode.BadRequest); }
                if (success) result = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }
        
       
       
    }
}
