using IPInfoProvider.Types.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPInfoProvider.Interfaces
{
   public interface IIPInfoProviderSQLRepository
    {
        IPDetails GetDetails(string ip);

        bool IpExists(string ip);

        bool IpExists(int id);

        bool CreateIP(IPDetails ipDetails);

        bool Save();

        bool UpdateIpDetails(IPDetails ipDetails);
    }
}
