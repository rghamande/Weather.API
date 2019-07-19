# Weather.API

A simple WebAPI that provides weather information from various sources such as OpenWeather, AccuWeather, WeatherBit.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

The curent implementation is limited to using OpenWeather provider.
Using WeatherAPI, you can retrieve current weather information for a specific city or list of cities at once.


### Prerequisites

* Visual Studio 2019
* Valid OpenWeather API subscription



### Installing

Current repository is built using Visual Studio 2019 (DotNet Framework 4.7.x).
The installation is pretty simple. Just download/clone the latest repository using visual studio.
The API uses OpenWeather provider to get the weather information.

In the web.config file update the "openWeatherAPIKey"key with your OpenWeather subscription key.
Know more about [OpenWeather API](https://openweathermap.org/api)
``` 
 <add key="OpenWeatherAPIKey" value="YOUR_OPENWEATHER_API_KEY"/>
```

```
GET api/WeatherInfo?cityId={cityId}
```
Get method takes a "cityId" as parameter and returns the data for a specific city.

```
GET api/WeatherInfo
```
GetAll method get's the current weather information for all the cities from the master "city.json" file. The response/Weatherinfo is saved in the "WeatherInfo" folder categories by cityId and date. The historical weather information is maintained as well.

```
POST api/WeatherInfo/GetMany
```
This method can be used to get the weather inforamtion for selected list of cities. method expects a **.txt** file in the request body.

Sample content of .txt file
```
[
  {
    "id": "2643741",
    "cityName": "City of London"
  },
  {
    "id": "2988507",
    "cityName": "Paris"
  }
]
```

The response/Weatherinfo is saved in the "WeatherInfo" folder categories by cityId and date. The historical weather information is maintained as well.

The Output path is configurable and can be modified using "APIResponsePath" key in the web.config. 

## Enhancements
* Implementing mechanism to store responses over the cloud storage such as Microsoft Azure, AWS etc.
* Implementation for other weather forcast providers.
* Enabling authentication for the API.

## Pending
Unit tests

