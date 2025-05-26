using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Xml.Linq;
using CanonCameraWebSite.Classes;

namespace CanonCameraWebSite.Pages
{
    public class IndexModel : PageModel
    {
        WeatherAPI wapi = new WeatherAPI();

        string response;

        [BindProperty]
        public string CameraSetting { get; set; }
        public Root WeatherDetails { get; set;}

        public string WeatherSetting { get; set; }

        TemperatureDetails tempDetails = new TemperatureDetails();
        WindDetails windDetails = new WindDetails();
        RainDetails rainDetails = new RainDetails();
        CloudDetails cloudDetails = new CloudDetails();
        public TemperatureDetails temperatureDetails { get; set; }   

        public string test {  get; set; }

        Dictionary<string, string> Wind { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public double CurrentTemp { get; set; }
        public double HeatIndex { get; set; }
        public double FeelsLike { get; set; }
        public string WindGust { get; set; }
        public string WindMph { get; set; }
        public string WindDirection { get; set; }
        public string WindDegree { get; set; }
        public string WindChill { get; set; }
        public double Precipitation { get; set; }
        public int Cloud {  get; set; }
        public string ApiErrorMessage { get; set; }
        public void OnGet()
        {

        }

        public void ProcessTemperatueDate(Dictionary<string, double> TemperatureData)
        {
            foreach (var item in TemperatureData)
            {
                if (item.Key == "Current Temp")
                {
                    tempDetails.CurrentTemp = (double)item.Value;
                }
                if (item.Key == "Heat Index")
                {
                    tempDetails.HeatIndex = (double)item.Value;
                }
                if (item.Key == "Feels Like")
                {
                    tempDetails.FeelsLike = (double)item.Value;
                }
            }
        }

        public void ProcessWindData(Dictionary<string, string>  WindData)
        {
            foreach (var item in WindData)
            {
                if (item.Key == "Wind Gust")
                {
                    windDetails.WindGust = item.Value.ToString();
                }
                if (item.Key == "Wind mph")
                {
                    windDetails.WindMph = item.Value.ToString();
                }
                if (item.Key == "Wind Direction")
                {
                    windDetails.WindDirection = item.Value.ToString();
                }
                if (item.Key == "Wind Degree")
                {
                    windDetails.WindDegree = item.Value.ToString();
                }
                if (item.Key == "Wind Chill")
                {
                    windDetails.WindChill = item.Value.ToString();
                }
            }
        }

        public void GetWeatherDetails(string weather, string location)
        {
            Dictionary<string, double> TempData = new Dictionary<string, double>();
            Dictionary<string, string> WindData = new Dictionary<string, string>();

            WeatherAPI wapi = new WeatherAPI();
            var currentWeather = wapi.getLocalWeather(location).Result;

            if (currentWeather.current == null)
            {
                ApiErrorMessage = "Please enter a valid location";
            }
            else
            {

                WindData = wapi.GetWindConditions(currentWeather);
                TempData = wapi.GetTemperatureConditions(currentWeather);

                switch (weather)
                {
                    case "Wind":
                        ProcessWindData(WindData);
                        break;
                    case "Rain":
                        rainDetails.Precipitation = wapi.GetRainConditions(currentWeather);
                        break;
                    case "Temperature":
                        ProcessTemperatueDate(TempData);
                        break;
                    case "Cloud":
                        wapi.GetCloudConditions(currentWeather);
                        cloudDetails.Cloud = wapi.GetCloudConditions(currentWeather);
                        break;
                    case "All Weather":
                        ProcessWindData(WindData);
                        ProcessTemperatueDate(TempData);
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnPost()
        {
            WeatherSetting = Request.Form["weatherattributes"];
            var requiredWeatherAttribute = Request.Form["weatherattributes"];
            GetWeatherDetails(requiredWeatherAttribute, Request.Form["location"]);
            CurrentTemp = tempDetails.CurrentTemp;
            FeelsLike = tempDetails.FeelsLike;
            HeatIndex = tempDetails.HeatIndex;
            WindGust = windDetails.WindGust;
            WindMph = windDetails.WindMph;
            WindDirection = windDetails.WindDirection;
            WindDegree = windDetails.WindDegree;
            WindChill = windDetails.WindChill;
            Precipitation = rainDetails.Precipitation;
        
            ViewData["confirmation"] = "information will be sent to";
        }


        //be1a9b09d93c40ebbf1193458251102
        public string GetCameraSetting(string camerasetting)
        {
            switch (camerasetting)
            {
                case "ISO":
                    response = "ISO";
                    break;

                case "Aperture":
                    response = "Aperture";
                    break;
            }
            return response;
        }

    }

    public class TemperatureDetails()
    {
        public double CurrentTemp { get; set; }
        public double HeatIndex { get; set; }
        public double FeelsLike { get; set; }
    }

    public class WindDetails()
    {
        public string WindGust { get; set; }
        public string WindMph { get; set; }
        public string WindDirection { get; set; }
        public string WindDegree { get; set; }
        public string WindChill { get; set; }
    }

    public class RainDetails()
    {
        public double Precipitation { get; set; }
    }

    public class CloudDetails()
    {
        public int Cloud { get; set; }
    }
}
