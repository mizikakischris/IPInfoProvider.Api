using IPInfoProvider.Types.Models;
using System;
using System.Threading.Tasks;

namespace IPInfoProvider.Interfaces
{
    public interface IIPInfoProviderService
    {
        Task<IPDetails> GetDetailsAsync(string ip);
    }
}
