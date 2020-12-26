using IPInfoProvider.Helpers;
using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPInfoProvider.Services
{
    public class IPInfoProviderService : IIPInfoProviderService
    {
        private readonly IHttpClientFactory _client;
        private AppSettings _settings;
        private readonly IMemoryCache _cache;
        private readonly IIPInfoProviderSQLRepository _sqlRepo;

        public IPInfoProviderService(IHttpClientFactory client, IOptions<AppSettings> settings, IMemoryCache cache, IIPInfoProviderSQLRepository sqlRepo)
        {
            _client = client;
            _settings = settings.Value;
            _cache = cache;
            _sqlRepo = sqlRepo;
        }
        public async Task<IPDetails> GetDetailsAsync(string ip)
        {
            //check if ip exists in cache
           bool cacheExists =  ExistsInCache(ip, _cache);
            if (!cacheExists)
            {
                
                if (!_sqlRepo.IpExists(ip)) 
                {
                    //Request data from external api: IPStack
                    var client = _client.CreateClient();
                    using (var request = new HttpRequestMessage(HttpMethod.Get, _settings.BaseUrl))
                    {
                        Helper.BuildHttpRequestMessage(request, ip, _settings.BaseUrl, _settings.AccessKey);

                        using (var response = await client.SendAsync(request))
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var responseObject = DeserializeResponse<IPDetails>(content);

                            _cache.Set<string>(ip, content);
                            return responseObject;

                        }
                    }
                }
               
            }
            else 
            {
                var content =  _cache.Get<string>(ip);
                var responseObject = DeserializeResponse<IPDetails>(content);

                return responseObject;
            }


        }

        private bool ExistsInCache(string ip, IMemoryCache cache)
        {
           
            return cache.TryGetValue<string>(ip, out var result);
              
        }

        private static T DeserializeResponse<T>(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (JsonException ex)
            {
                throw new Exception("Σφάλμα κατά την ανάκτηση τιμών από το IPStack.");
            }
        }
    }

}
