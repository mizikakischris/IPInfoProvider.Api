using AutoMapper;
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
        private readonly IMapper _mapper;

        public IPInfoProviderService(
            IHttpClientFactory client, 
            IOptions<AppSettings> settings, IMemoryCache cache, IIPInfoProviderSQLRepository sqlRepo, IMapper mapper)
        {
            _client = client;
            _settings = settings.Value;
            _cache = cache;
            _sqlRepo = sqlRepo;
            _mapper = mapper;
        }
        public async Task<IPDetailsDto> GetDetailsAsync(string ip)
        {
            //check if ip exists in cache
           bool cacheExists =  ExistsInCache(ip, _cache);
            if (!cacheExists)
            {
                //check if exists in DB
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
                            var deserializedDto = DeserializeResponse<IPDetailsDto>(content);

                            //write to Db
                            var ipDetailsModel = _mapper.Map<IPDetails>(deserializedDto);
                            ipDetailsModel.IP = ip;
                            _sqlRepo.CreateIP(ipDetailsModel);

                            var serializedDto = JsonConvert.SerializeObject(deserializedDto);
                            _cache.Set<string>(ip, serializedDto);
                            return deserializedDto;

                        }
                    }
                }
                else 
                {
                    //fetch from db
                   var ipDetailsModel = _sqlRepo.GetDetails(ip);
                    var ipDetailsDto = _mapper.Map<IPDetailsDto>(ipDetailsModel);
                   var serializedDto =  JsonConvert.SerializeObject(ipDetailsDto);

                    //put data in cache
                    _cache.Set<string>(ip, serializedDto);

                    return ipDetailsDto;
                }
               
            }
            else 
            {
                var content =  _cache.Get<string>(ip);
                var deserializedDto = DeserializeResponse<IPDetailsDto>(content);

                return deserializedDto;
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
