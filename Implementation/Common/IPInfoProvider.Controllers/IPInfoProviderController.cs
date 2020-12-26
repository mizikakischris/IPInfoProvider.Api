using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using Microsoft.AspNetCore.Mvc;
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
    }
    [DataContract]
    public class GetDetailsRequest
    {
        [DataMember (Name ="ip")]
        public string  IP { get; set; }
    }
}
