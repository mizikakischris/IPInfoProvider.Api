using System;
using System.Runtime.Serialization;

namespace IPInfoProvider.Exceptions
{
    [DataContract]
    public class BadRequestException : Exception
    {
        public BadRequestException(string exception): base(exception)
        {
        }
        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
