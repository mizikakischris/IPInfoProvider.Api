using System.Collections.Generic;
using IPInfoProvider.Exceptions;
using System.Runtime.Serialization;
using System;

namespace IPInfoProvider.Types.Responses
{
    [DataContract]
    public class Response<T> where T : class
    {
        [DataMember]
        public string Version { get { return "1.1"; } }

        [DataMember(Name = "payload")]
        public Payload<T> Payload { get; set; }

        [DataMember(Name = "guid")]
        public Guid Guid { get; set; }
        [DataMember(Name = "exception")]
        public ErrorDetails Exception { get; set; }

    }


    public class Payload<T> where T: class 
    {
        [DataMember(Name = "ipDetails")]
        public T IPDetails { get; set; }

        [DataMember(Name = "responseUpdate")]
        public T ResponseUpdate { get; set; }

    }
}
