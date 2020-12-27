using AutoMapper;
using IPInfoProvider.Exceptions;
using IPInfoProvider.Helpers;
using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using IPInfoProvider.Types.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
        public async Task<Response<IPDetailsDto>> GetDetailsAsync(string ip)
        {
            //check if ip exists in cache
            bool cacheExists = ExistsInCache(ip, _cache);
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
                            if (!response.IsSuccessStatusCode)
                                await Helper.HandleNonOkAsync(response);
                            var content = await response.Content.ReadAsStringAsync();
                            var deserializedDto = Helper.DeserializeResponse<IPDetailsDto>(content);

                            //write to Db
                            var ipDetailsModel = _mapper.Map<IPDetails>(deserializedDto);
                            ipDetailsModel.IP = ip;
                            _sqlRepo.CreateIP(ipDetailsModel);

                            var serializedDto = JsonConvert.SerializeObject(deserializedDto);
                            _cache.Set<string>(ip, serializedDto);
                            Response<IPDetailsDto> responseObj = Helper.BuildResponse(deserializedDto);
                            return responseObj;

                        }
                    }
                }
                else
                {
                    //fetch from db
                    var ipDetailsModel = _sqlRepo.GetDetails(ip);
                    var ipDetailsDto = _mapper.Map<IPDetailsDto>(ipDetailsModel);
                    var serializedDto = JsonConvert.SerializeObject(ipDetailsDto);

                    //put data in cache
                    _cache.Set<string>(ip, serializedDto);
                    Response<IPDetailsDto> responseObj = Helper.BuildResponse(ipDetailsDto);
                    return responseObj;
                }

            }
            else
            {
                var content = _cache.Get<string>(ip);
                var deserializedDto = Helper.DeserializeResponse<IPDetailsDto>(content);
                Response<IPDetailsDto> responseObj = Helper.BuildResponse(deserializedDto);
                return responseObj;
            }


        }
        private bool ExistsInCache(string ip, IMemoryCache cache)
        {

            return cache.TryGetValue<string>(ip, out var result);

        }


        public async Task<Response<IPDetailsDto>> UpdateIPDetails(List<IPDetailsDto> ipDetailsList)
        {
            ValidateIPDetails(ipDetailsList);
           var res = StartProcessing(ipDetailsList);

            Response<IPDetailsDto> resp = new Response<IPDetailsDto>
            {
                Guid = res.Keys.First()
            };
            return resp;
        }

        private Dictionary<Guid, double>  StartProcessing(List<IPDetailsDto> ipDetailsList)
        {
            int counter = 0;
            int sizeToFetch = 2;
            int total = 0;
            int skipNumber = 0;
            var sortedList = ipDetailsList.OrderBy(x => x.Country).ToList();
            double result = 0;
            Dictionary<Guid, double> dict = new Dictionary<Guid, double>();

            var bufferBlock = new BufferBlock<IPDetailsDto>();


            while (total < ipDetailsList.Count)
            {
                if (counter == 0)
                {
                    counter++;

                    //Take 10
                    var list = sortedList.Take(2).ToList();

                    PostToBuffer(bufferBlock, list);

                    //Update DB
                    ReceiveFromBuffer(bufferBlock, list);

                    total += sizeToFetch; 
                    skipNumber += sizeToFetch; 

                }
                else
                {
                    var list = ipDetailsList.Skip(skipNumber).Take(sizeToFetch).ToList();
                    PostToBuffer(bufferBlock, list);

                    //Update DB
                    ReceiveFromBuffer(bufferBlock, list);

                    total += sizeToFetch; // 20
                    result = (double)total / sortedList.Count;
                    skipNumber += sizeToFetch; // 20
                }
             
            }
             dict.Add(Guid.NewGuid(), result);
            return dict;

        }

        private void ReceiveFromBuffer(BufferBlock<IPDetailsDto> bufferBlock, List<IPDetailsDto> list)
        {
            foreach (var item in list)
            {
                var received = bufferBlock.Receive();

                //update cache
                var serializedDto = JsonConvert.SerializeObject(received);
                _cache.Set<string>(received.IP, serializedDto);

                //update db
                var ipDetailsModel = _mapper.Map<IPDetails>(item);

                var res = _sqlRepo.UpdateIpDetails(ipDetailsModel);
            }
        }

        private static void PostToBuffer(BufferBlock<IPDetailsDto> bufferBlock, List<IPDetailsDto> list)
        {
            foreach (var item in list)
            {
                bufferBlock.Post(item);
            }
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
        private bool SubmitToDatabase(List<IPDetailsDto> ipDetailsList)
        {

            foreach (var item in ipDetailsList)
            {

                var ipDetailsModel = _mapper.Map<IPDetails>(item);

               var res=  _sqlRepo.UpdateIpDetails(ipDetailsModel);
               
            }
            return true;
        }
    }

}
