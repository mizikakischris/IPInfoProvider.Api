using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IPInfoProvider.Repository
{
    public class IPInfoProviderSQLRepository : IIPInfoProviderSQLRepository
    {
        public Task<IPDetails> GetDetailsAsync(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
