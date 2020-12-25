using System;
using System.Runtime.Serialization;

namespace IPInfoProvider.Types.Models
{
    [DataContract]
    public class IPDetails
    {
        public int Id { get; set; }

        [DataMember(Name ="city")]
        public string  City { get; set; }
        [DataMember(Name = "country")]
        public string Country { get; set; }
        [DataMember(Name = "continent")]
        public string Continent { get; set; }
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }
        [DataMember(Name = "longitude")]
        public string Longitude { get; set; }
    }
}
