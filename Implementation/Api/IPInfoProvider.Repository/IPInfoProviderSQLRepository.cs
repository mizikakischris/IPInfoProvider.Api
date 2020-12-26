using IPInfoProvider.Interfaces;
using IPInfoProvider.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPInfoProvider.Repository
{
    public class IPInfoProviderSQLRepository : IIPInfoProviderSQLRepository
    {
        private readonly AppDbContext _db;
        public IPInfoProviderSQLRepository(AppDbContext db)
        {
            _db = db;
        }
        public IPDetails GetDetails(string ip)
        {
           return  _db.IPDetails.Where(x => x.IP == ip).FirstOrDefault();
          
        }
    }
}
