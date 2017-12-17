using Newtonsoft.Json;
using ServiceResultModels;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Proxies
{
    public class DefaultServiceProxy : IServiceProxy
    {
        private static HttpClient _httpClient = new HttpClient();
        public IList<UserModel> GetAll()
        {
            var response = _httpClient.GetAsync("http://localhost:8010/api/DefaultService").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<UserModel>>(content);
            }
        }
    }
}
