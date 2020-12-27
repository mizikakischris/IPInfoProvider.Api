using IPInfoProvider.Types.Models;

namespace IPInfoProvider.Interfaces
{
   public interface IIPInfoProviderSQLRepository
    {
        IPDetails GetDetails(string ip);

        bool IpExists(string ip);

        bool CreateIP(IPDetails ipDetails);

        bool Save();

        bool UpdateIpDetails(IPDetails ipDetails);
    }
}
