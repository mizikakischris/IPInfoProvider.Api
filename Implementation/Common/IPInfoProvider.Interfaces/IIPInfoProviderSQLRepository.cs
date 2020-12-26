using IPInfoProvider.Types.Models;
using System.Threading.Tasks;

namespace IPInfoProvider.Interfaces
{
   public interface IIPInfoProviderSQLRepository
    {
        IPDetails GetDetails(string ip);
    }
}
