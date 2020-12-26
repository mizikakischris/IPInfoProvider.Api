using IPInfoProvider.Helpers;
using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
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

        public IPInfoProviderService(IHttpClientFactory client, IOptions<AppSettings> settings)
        {
            _client = client;
            _settings = settings.Value;
        }
        public async Task<IPDetails> GetDetailsAsync(string ip)
        {
            var client = _client.CreateClient();
            using (var request = new HttpRequestMessage(HttpMethod.Get, _settings.BaseUrl))
            {
                Helper.BuildHttpRequestMessage(request, ip, _settings.BaseUrl, _settings.AccessKey);


                using (var response = await client.SendAsync(request))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseObject = DeserializeResponse<IPDetails>(content);
                    return responseObject;

                }
            }

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
