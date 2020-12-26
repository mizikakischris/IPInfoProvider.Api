using AutoMapper;
using IPInfoProvider.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPInfoProvider.Api.Mapper
{
    public class IpDetailsMappings : Profile
    {
        public IpDetailsMappings()
        {
            CreateMap<IPDetails, IPDetailsDto>().ReverseMap();
        
        }
    }
}
