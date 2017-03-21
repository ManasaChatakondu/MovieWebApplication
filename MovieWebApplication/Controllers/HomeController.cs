using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MovieWebApplication.Models;

namespace MovieWebApplication.Controllers
{
    public class HomeController : Controller
    {
        string cinemaurl = "/api/cinemaworld/movies";
        string filmurl = "/api/filmworld/movies";
        
        // GET: Home
        public async Task<ActionResult> Index()
        {
            ViewBag.Movies = "";
            MoviesList cinemaModel = null;
            MoviesList filmModel = null;
            List<MovieModel> model = new List<MovieModel>();
            string jsonCinemadata = await GetMoviesbyUrl(cinemaurl);
            string jsonFilmdata = await GetMoviesbyUrl(filmurl);

            if (jsonCinemadata == "NoRecords" && jsonFilmdata == "NoRecords")
            {
                ViewBag.Movies = "No Movies";
                return View();
            }
            if (jsonCinemadata != "NoRecords")
            {
                cinemaModel = JsonConvert.DeserializeObject<MoviesList>(jsonCinemadata);
                model.AddRange(cinemaModel.Movies);
            }
            if (jsonFilmdata != "NoRecords")
            {
                filmModel = JsonConvert.DeserializeObject<MoviesList>(jsonFilmdata);
                model.AddRange(filmModel.Movies);
            }
            return View(model);
        }
        private async Task<string> GetMoviesbyUrl(string url)
        {
            var webApi = new MovieApiController();
            return await webApi.GetMovies(url);
        }
    }
}