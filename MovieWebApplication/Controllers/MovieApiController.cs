using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using MovieWebApplication.Models;

namespace MovieWebApplication.Controllers
{
    public class MovieApiController : ApiController
    {
        public async Task<string> GetMovies(string url)
        {
            /*Movie Details*/
            string apiBaseUri = "http://webjetapitest.azurewebsites.net";
            string token = ConfigurationManager.AppSettings["Token"];
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("x-access-token", token);

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return data;
                    }
                    else
                        return "NoRecords";
                }
            }
            catch (HttpRequestException ex)
            {
                //This handles HttpClient exceptions
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
            return "NoRecords";
        }
    }
}
