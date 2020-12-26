using System;
using System.Runtime.Serialization;

namespace IPInfoProvider.Types.Models
{
    [DataContract]
    public class IPDetailsDto
    {

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "country_name")]
        public string Country { get; set; }

        [DataMember(Name = "continent_name")]
        public string Continent { get; set; }
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }

        [DataMember(Name = "longitude")]
        public string Longitude { get; set; }

        [DataMember(Name = "ip")]
        public string IP { get; set; }

    }
}
