using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWebApplication.Controllers
{
    public interface IHttpClient
    {
        System.Threading.Tasks.Task<T> DeleteAsync<T>(string uri) where T : class;
        System.Threading.Tasks.Task<T> DeleteAsync<T>(Uri uri) where T : class;
        System.Threading.Tasks.Task<T> GetAsync<T>(string uri) where T : class;
        System.Threading.Tasks.Task<T> GetAsync<T>(Uri uri) where T : class;
        System.Threading.Tasks.Task<T> PostAsync<T>(string uri, object package);
        System.Threading.Tasks.Task<T> PostAsync<T>(Uri uri, object package);
        System.Threading.Tasks.Task<T> PutAsync<T>(string uri, object package);
        System.Threading.Tasks.Task<T> PutAsync<T>(Uri uri, object package);
    }
}
