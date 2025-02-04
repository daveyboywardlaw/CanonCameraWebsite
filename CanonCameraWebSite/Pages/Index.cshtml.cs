using CanonCameraWebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace CanonCameraWebSite.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string CameraSetting { get; set; }

        public string setup;
        public string punchline;
        public string type;

        string response;

        public string Message { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var dave = Request.Form["camerasetting"];


            HttpClient client = new HttpClient();
            var response = client.GetStringAsync("https://official-joke-api.appspot.com/random_joke").Result;
            var joke = JsonConvert.DeserializeObject<Joke>(response);
            setup = joke.Setup;
            punchline = joke.Punchline;
            type = joke.Type;

            ViewData["confirmation"] = $"{setup}, information will be sent to {punchline}";
            Message = GetCameraSetting(dave);
        }

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
}

        public class Joke
        {
            public string Setup { get; set; }
            public string Punchline { get; set; }
            public string Type { get; set; }

        }
