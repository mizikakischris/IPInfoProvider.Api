using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace IPInfoProvider.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class IPInfoProviderController : ControllerBase
    {
        private readonly IIPInfoProviderService _service;
        public IPInfoProviderController(IIPInfoProviderService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("GetDetails")]
        public async Task<ActionResult<IPDetailsDto>> GetDetails(GetDetailsRequest request)
        {
            var response = await _service.GetDetailsAsync(request.IP);
            return Ok(response);
        }

        [HttpPatch("{ip:int}")]
        [Route("UpdateDetails")]
        public ActionResult<Guid> UpdateIPDetails(List<IPDetailsDto> detailsDtoList)
        {

            var response = _service.UpdateIPDetails(detailsDtoList);
            return Ok(response);
        }

    }





    [DataContract]
    public class GetDetailsRequest
    {
        [DataMember(Name = "ip")]
        public string IP { get; set; }
    }
}
