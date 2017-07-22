using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleSignalR_001.WebApi
{
    public static class Utils
    {
        static HttpClient client = new HttpClient();

        public static async Task<T> GetAsync<T>(string url, string action)
        {
            T result = default(T);
            HttpResponseMessage response = await client.GetAsync(url + action);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(json);
            }
            //result = await response.Content.ReadAsAsync<T>();
            return result;
        }

        public static async Task<O> PostAsync<O, T>(string url, string action, T entity) //where U: 
        {
            O result = default(O);
            HttpResponseMessage response = await client.PostAsJsonAsync(url + action, entity);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<O>(json);
            }
            //result = await response.Content.ReadAsAsync<O>();
            return result;
        }
    }
}
