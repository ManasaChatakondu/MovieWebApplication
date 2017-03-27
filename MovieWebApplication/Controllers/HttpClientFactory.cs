using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MovieWebApplication.Controllers
{
    public interface IHttpClientFactory
    {
        HttpClient GetInstance();
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        public string ApiBaseUrl { get; set; }
        public string Token { get; set; }

        public HttpClientFactory()
        {
            Token = ConfigurationManager.AppSettings["Token"];
            ApiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        }

        public HttpClient GetInstance()
        {
            var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-access-token", Token);
            return client;
        }
    }
}