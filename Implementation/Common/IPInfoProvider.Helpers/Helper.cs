using IPInfoProvider.Types.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
    }
}
