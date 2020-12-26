using IPInfoProvider.Types.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPInfoProvider.Interfaces
{
    public interface IIPInfoProviderService
    {
        Task<IPDetailsDto> GetDetailsAsync(string ip);

        Guid UpdateIPDetails(List<IPDetailsDto> ipDetailsList);
    }
}
