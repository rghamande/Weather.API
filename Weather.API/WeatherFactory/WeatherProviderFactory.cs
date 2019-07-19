using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Weather.API.Models;
using Weather.API.ServiceProviders.OpenWeather;

namespace Weather.API.WeatherFactory
{
    abstract class WeatherProviderFactory
    {
       public  abstract IWeatherProvider FactoryMethod(string provider);
    }

    class WeatherProvider : WeatherProviderFactory
    {
        public override IWeatherProvider FactoryMethod(string provider)
        {
            if (!string.IsNullOrEmpty(provider))
            {
                switch (provider)
                {
                    case "OpenWeather":
                      return  new OpenWeather();
                    default:
                        //Set Default Weather provider here
                        return new OpenWeather();
                }
            }
            // throw "No service provider" exception
            // Currently returning OpenWeather service instance.
            return new OpenWeather();
        }
    }
}