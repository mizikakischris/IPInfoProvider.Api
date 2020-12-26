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

        public bool CreateIP(IPDetails ipDetails)
        {
            _db.IPDetails.Add(ipDetails);
            return Save();
        }

        public IPDetails GetDetails(string ip)
        {
           return  _db.IPDetails.Where(x => x.IP == ip).FirstOrDefault();
          
        }

        public bool IpExists(string ip)
        {
            bool value = _db.IPDetails.Any(a => a.IP.ToLower().Trim() == ip.ToLower().Trim());
            return value;
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

    }
}
