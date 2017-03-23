using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MovieWebApplication.Models;

namespace MovieWebApplication.Controllers
{
    public class HomeController : Controller
    {
        string cinemaurl = "/api/cinemaworld/movies";
        string filmurl = "/api/filmworld/movies";
        MovieApiController webApi = new MovieApiController();
        static MoviesList cinemaModel = null;
        static MoviesList filmModel = null;

        public async Task<ActionResult> Index()
        {
            ViewBag.Movies = "";
            List<MovieModel> model = new List<MovieModel>();
            List<CinemaFilmVM> cfModel = new List<CinemaFilmVM>();
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
            //Adding Title and Poster to a list,to maintain unique records.
            foreach(MovieModel movie in model)
            {
                bool containsItem = cfModel.Any(item => item.Title == movie.Title);//Checking if the movietitle is already there in the list
                if(!containsItem)
                    cfModel.Add(new CinemaFilmVM { Title= movie.Title,Poster= movie.Poster });
            }
            return View(cfModel);
        }       

        public async Task<ActionResult> GetMovieByID(string title,string poster)
        {
            string cinemaUrl, cinemaData = null, cinemaPrice = null; 
            string filmUrl, filmData = null, filmPrice=null;
            if (cinemaModel!=null)
            {
                foreach(MovieModel movie in cinemaModel.Movies)
                {
                    if(movie.Title== title)
                    {
                        cinemaUrl = "/api/cinemaworld/movie/" + movie.ID;
                        cinemaData = await webApi.GetMovies(cinemaUrl);
                        if(cinemaData!= "NoRecords")
                        {
                            dynamic cinema = JObject.Parse(cinemaData);
                            cinemaPrice = cinema.Price;
                            break;
                        }
                    }
                }
            }
            if (filmModel != null)
            {
                foreach (MovieModel movie in filmModel.Movies)
                {
                    if (movie.Title == title)
                    {
                        filmUrl = "/api/filmworld/movie/" + movie.ID;
                        filmData = await webApi.GetMovies(filmUrl);
                        if (filmData != "NoRecords")
                        {
                            dynamic film = JObject.Parse(filmData);
                            filmPrice = film.Price;
                            break;
                        }
                    }
                }
            }
            if (cinemaPrice != null && filmPrice != null)
            {
                if (Convert.ToDouble(cinemaPrice) <= Convert.ToDouble(filmPrice))
                    return RedirectToAction("MovieDescription", new { data = cinemaData });
                else
                    return RedirectToAction("MovieDescription", new { data = filmData });
            }
            else if (cinemaPrice != null)
                return RedirectToAction("MovieDescription", new { data = cinemaData });
            else if (filmPrice != null)
                return RedirectToAction("MovieDescription", new { data = filmData });
            else
                return RedirectToAction("MovieDescription", new { data = "NoData" });
        }

        public ActionResult MovieDescription(string data)
        {
            ViewBag.Data = "";
            MovieDescModel model = new MovieDescModel();
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

        private async Task<string> GetMoviesbyUrl(string url)
        {
            return await webApi.GetMovies(url);
        }        
    }
}