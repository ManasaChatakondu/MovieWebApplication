using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace MovieWebApplication.Controllers
{
    public class MovieApiController : ApiController
    {
        public IHttpClientFactory Factory { get; set; }

        public MovieApiController()
        {
            Factory = new HttpClientFactory();
        }

        public MovieApiController(IHttpClientFactory factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// Method to get the list of movies
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetMovies(string url)
        {
            try
            {
                using (var client = Factory.GetInstance())
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return data;
                    }
                    return "NoRecords";
                }
            }
            catch (HttpRequestException ex)
            {
                return $"Message :{ex.Message} ";
            }
        }
    }
}
