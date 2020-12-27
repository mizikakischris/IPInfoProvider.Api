using IPInfoProvider.Exceptions;
using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using IPInfoProvider.Types.Responses;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Response<IPDetailsDto>>> GetDetails(GetDetailsRequest request)
        {
            var response = await _service.GetDetailsAsync(request.IP);
            return Ok(response);
        }

        [HttpPatch]
        [Route("UpdateDetails")]
        public async Task<ActionResult<Response<IPDetailsDto>>> UpdateIPDetails(List<IPDetailsDto> detailsDtoList)
        {
            try
            {
                var response = await _service.UpdateIPDetails(detailsDtoList);
                return Ok(response);
            }
            catch (ErrorDetails ex)
            {
                Response<IPDetailsDto> resp = new Response<IPDetailsDto>
                {
                    Payload = null,
                    Exception = ex
                };
                return resp;
            }
        
        }

    }





    [DataContract]
    public class GetDetailsRequest
    {
        [DataMember(Name = "ip")]
        public string IP { get; set; }
    }
}
