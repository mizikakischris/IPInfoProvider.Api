using IPInfoProvider.Exceptions;
using IPInfoProvider.Types.Models;
using IPInfoProvider.Types.Responses;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IPInfoProvider.Helpers
{
    public static class Helper
    {
        public static void BuildHttpRequestMessage(HttpRequestMessage request, string ip, string baseUrl, string accessKey)
        {
            request.RequestUri = new Uri(baseUrl + ip + "?access_key=" + accessKey);
            request.Headers
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task HandleNonOkAsync(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException("Σφάλμα κατά την ανάκτηση τιμών από το IPStack - Bad Request Exception");
                default:
                    throw new Exception("Σφάλμα κατά την ανάκτηση τιμών από IPStack - Internal Server Error Exception");
            }
        }

        public static Response<IPDetailsDto> BuildResponse(IPDetailsDto deserializedDto)
        {
            return new Response<IPDetailsDto>
            {
                Payload = new Payload<IPDetailsDto>
                {
                    IPDetails = deserializedDto
                }
            };
        }


        public static T DeserializeResponse<T>(string content)
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
