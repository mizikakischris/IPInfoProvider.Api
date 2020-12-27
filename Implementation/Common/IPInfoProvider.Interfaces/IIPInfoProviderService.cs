using IPInfoProvider.Types.Models;
using IPInfoProvider.Types.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPInfoProvider.Interfaces
{
    public interface IIPInfoProviderService
    {
        Task<Response<IPDetailsDto>> GetDetailsAsync(string ip);

        Task <Response<IPDetailsDto>> UpdateIPDetails(List<IPDetailsDto> ipDetailsList);
    }
}
