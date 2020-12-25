using IPInfoProvider.Types.Models;
using System;

namespace IPInfoProvider.Interfaces
{
    public interface IIPInfoProviderService
    {
        IPDetails GetDetails(string ip);
    }
}
