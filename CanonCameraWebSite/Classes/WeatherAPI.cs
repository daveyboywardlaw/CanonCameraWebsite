using Newtonsoft.Json;
using RestWrapper;
using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace CanonCameraWebSite.Classes
{
    public class WeatherAPI : IAPIValidation
    {
        bool ErrorReturned;

        public async Task<Root> getLocalWeather(string location)
        {
            APIKeys api = new APIKeys();
            var apiKey = api.WeatherAPIValue;


            //RestRequest request = new RestRequest("https://api.weatherapi.com/v1/current.json?q=" + location + "&key=be1a9b09d93c40ebbf1193458251102");

            RestRequest request = new RestRequest("https://api.weatherapi.com/v1/current.json?q=" + location + "&key=" + apiKey);

            var response = await request.SendAsync();
            Root currentWeather = response.DataFromJson<Root>();

            if (currentWeather == null)
            {
                currentWeather = null;
            }

            return currentWeather;
        }

        public string ErrorMessage { get; set; }

        public string ProcessErrorReponse(RestResponse response)
        {
            return "bob";
        }

        public Dictionary<string, string> GetWindConditions(Root WeatherDetails)
        {
            Dictionary<string,string> WindConditions = new Dictionary<string, string>();

            WindConditions.Add("Wind Gust", WeatherDetails.current.gust_mph.ToString());
            WindConditions.Add("Wind mph", WeatherDetails.current.wind_mph.ToString());
            WindConditions.Add("Wind Direction", WeatherDetails.current.wind_dir.ToString());
            WindConditions.Add("Wind Degree", WeatherDetails.current.wind_degree.ToString());
            WindConditions.Add("Wind Chill", WeatherDetails.current.windchill_c.ToString());

            return WindConditions;
        }

        public int GetCloudConditions(Root WeatherDetails)
        {
            return WeatherDetails.current.cloud;
        }

        public Dictionary<string,double> GetTemperatureConditions(Root WeatherDetails)
        {
            Dictionary<string, double> Temperature = new Dictionary<string, double>();

            Temperature.Add("Current Temp", WeatherDetails.current.temp_c);
            Temperature.Add("Heat Index", WeatherDetails.current.heatindex_c);
            Temperature.Add("Feels Like", WeatherDetails.current.feelslike_c);

            return Temperature;
        }

        public double GetRainConditions(Root WeatherDetails)
        {
            return WeatherDetails.current.precip_mm;
        }
    }
}

public class Condition
{
    public string text { get; set; }
    public string icon { get; set; }
    public int code { get; set; }
}

public class Current
{
    public int last_updated_epoch { get; set; }
    public string last_updated { get; set; }
    public double temp_c { get; set; }
    public double temp_f { get; set; }
    public int is_day { get; set; }
    public Condition condition { get; set; }
    public double wind_mph { get; set; }
    public double wind_kph { get; set; }
    public int wind_degree { get; set; }
    public string wind_dir { get; set; }
    public double pressure_mb { get; set; }
    public double pressure_in { get; set; }
    public double precip_mm { get; set; }
    public double precip_in { get; set; }
    public int humidity { get; set; }
    public int cloud { get; set; }
    public double feelslike_c { get; set; }
    public double feelslike_f { get; set; }
    public double windchill_c { get; set; }
    public double windchill_f { get; set; }
    public double heatindex_c { get; set; }
    public double heatindex_f { get; set; }
    public double dewpoint_c { get; set; }
    public double dewpoint_f { get; set; }
    public double vis_km { get; set; }
    public double vis_miles { get; set; }
    public double uv { get; set; }
    public double gust_mph { get; set; }
    public double gust_kph { get; set; }
}

public class Location
{
    public string name { get; set; }
    public string region { get; set; }
    public string country { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
    public string tz_id { get; set; }
    public int localtime_epoch { get; set; }
    public string localtime { get; set; }
}

public class Root
{
    public Location location { get; set; }
    public Current current { get; set; }
}
