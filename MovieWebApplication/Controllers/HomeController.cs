using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MovieWebApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieApiController _webApi;
        private static MoviesList _cinemaModel;
        private static MoviesList _filmModel;

        #region Properties

        public string CinemaUrl { get; set; }
        public string FilmUrl { get; set; }

        #endregion
        
        public HomeController()
        {
            CinemaUrl = ConfigurationManager.AppSettings["CinemaUrl"];
            FilmUrl = ConfigurationManager.AppSettings["FilmUrl"];
            _webApi = new MovieApiController();
        }

        #region public Methods
        
        [HandleError]
        public async Task<ActionResult> Index()
        {
            ViewBag.Movies = "";
            var model = new List<MovieModel>();
            var cfModel = new List<CinemaFilmVM>();
            var jsonCinemadata = await GetMoviesbyUrl(CinemaUrl);
            var jsonFilmdata = await GetMoviesbyUrl(FilmUrl);

            if (jsonCinemadata == "NoRecords" && jsonFilmdata == "NoRecords")
            {
                ViewBag.Movies = "No Movies";
                return View();
            }
            if (jsonCinemadata != "NoRecords")
            {
                _cinemaModel = JsonConvert.DeserializeObject<MoviesList>(jsonCinemadata);
                model.AddRange(_cinemaModel.Movies);
            }
            if (jsonFilmdata != "NoRecords")
            {
                _filmModel = JsonConvert.DeserializeObject<MoviesList>(jsonFilmdata);
                model.AddRange(_filmModel.Movies);
            }
            //Adding Title and Poster to a list,to maintain unique records.
            foreach (var movie in model)
            {
                var containsItem = cfModel.Any(item => item.Title == movie.Title);
                //looping to Check if the movietitle is already there in the list
                if (!containsItem)
                    cfModel.Add(new CinemaFilmVM { Title = movie.Title, Poster = movie.Poster });
            }
            return View(cfModel);
        }

        [HandleError]
        public async Task<ActionResult> GetMovieById(string title, string poster)
        {
            string cinemaData = null, cinemaPrice = null;
            string filmData = null, filmPrice = null;
            if (_cinemaModel != null)
            {
                foreach (var movie in _cinemaModel.Movies)
                {
                    if (movie.Title == title)
                    {
                        var cinemaUrl = "/api/cinemaworld/movie/" + movie.ID;
                        cinemaData = await _webApi.GetMovies(cinemaUrl);
                        if (cinemaData != "NoRecords")
                        {
                            dynamic cinema = JObject.Parse(cinemaData);
                            cinemaPrice = cinema.Price;
                            break;
                        }
                    }
                }
            }
            if (_filmModel != null)
            {
                foreach (var filmUrl in from movie in _filmModel.Movies where movie.Title == title select "/api/filmworld/movie/" + movie.ID)
                {
                    filmData = await _webApi.GetMovies(filmUrl);
                    if (filmData == "NoRecords") continue;
                    dynamic film = JObject.Parse(filmData);
                    filmPrice = film.Price;
                    break;
                }
            }
            if (cinemaPrice != null && filmPrice != null)
            {
                return RedirectToAction("MovieDescription", Convert.ToDouble(cinemaPrice) <= Convert.ToDouble(filmPrice)
                    ? new { data = cinemaData } : new { data = filmData });
            }
            if (cinemaPrice != null)
                return RedirectToAction("MovieDescription", new { data = cinemaData });
            return RedirectToAction("MovieDescription", filmPrice != null
                ? new { data = filmData } : new { data = "NoData" });
        }

        public ActionResult MovieDescription(string data)
        {
            ViewBag.Data = "";
            var model = new MovieDescModel();
            if (data != "NoData")
            {
                dynamic cinema = JObject.Parse(data);
                model.Title = cinema.Title;
                model.Price = cinema.Price;
                model.Poster = cinema.Poster;
                model.Director = cinema.Director;
                model.Writer = cinema.Writer;
                model.Actors = cinema.Actors;
                model.Rating = cinema.Rating;
            }
            else
                ViewBag.Data = "No data exists";
            return View(model);
        }

        #endregion

        #region private methods

        private async Task<string> GetMoviesbyUrl(string url)
        {
            return await _webApi.GetMovies(url);
        }

        #endregion
    }
}