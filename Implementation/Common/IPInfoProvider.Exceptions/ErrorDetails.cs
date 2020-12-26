using System;
using System.Runtime.Serialization;

namespace IPInfoProvider.Exceptions
{
    [DataContract]
    public class ErrorDetails : Exception
    {
        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
