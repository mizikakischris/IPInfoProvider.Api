using AutoMapper;
using IPInfoProvider.Exceptions;
using IPInfoProvider.Helpers;
using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Guid UpdateIPDetails(List<IPDetailsDto> ipDetailsList)
        {
            ValidateIPDetails(ipDetailsList);
            StartProcessing(ipDetailsList);
            return Guid.NewGuid();
        }

        private void ValidateIPDetails(List<IPDetailsDto> detailsDtoList)
        {
            foreach (var item in detailsDtoList)
            {
                if (!_sqlRepo.IpExists(item.IP))
                {
                    throw new ErrorDetails
                    {
                        Description = $"IPDetails Not found for ip {item.IP}",
                        StatusCode = StatusCodes.Status404NotFound,
                    };
                }
            }
        }
        private void UpdateProcessing(List<IPDetailsDto> ipDetailsList)
        {
            
            foreach (var item in ipDetailsList)
            {

                var ipDetailsModel = _mapper.Map<IPDetails>(item);
                _sqlRepo.IpExists(ipDetailsModel.IP);
                _sqlRepo.UpdateIpDetails(ipDetailsModel);
            }
        }

        private void StartProcessing(List<IPDetailsDto> ipDetailsList)
        {
            int counter = 0;
            int sizeToFetch = 2;
            int total = 0;
            int skipNumber = 0;
            var sortedList = ipDetailsList.OrderBy(x => x.Country).ToList();
            float result = 0;
            Dictionary<Guid, float> dict = new Dictionary<Guid, float>();
            while (total<=ipDetailsList.Count)
            {
                if (counter == 0)
                {
                    counter++;
                    var list = sortedList.Take(2).ToList();
                    UpdateProcessing(list);
                    total += sizeToFetch; // total = 10
                    skipNumber += sizeToFetch; // skip = 10

                    result = total / sortedList.Count;
                    dict.Add(Guid.NewGuid(), result);
                    //event notify
                    //pause executio
                    //return guid 
                    //come back and continue process
                }
                else
                {
                    var list = ipDetailsList.Skip(skipNumber).Take(sizeToFetch).ToList();
                    UpdateProcessing(list);
                    total += sizeToFetch; // 20

                    result = total / sortedList.Count;
                    dict.Add(Guid.NewGuid(), result);

                    skipNumber += sizeToFetch; // 20
                }
            }
            
        }
    }

}
