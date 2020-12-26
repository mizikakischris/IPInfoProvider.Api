using IPInfoProvider.Types.Models;
using System.Threading.Tasks;

namespace IPInfoProvider.Interfaces
{
   public interface IIPInfoProviderSQLRepository
    {
        Task<IPDetails> GetDetailsAsync(string ip);
    }
}
