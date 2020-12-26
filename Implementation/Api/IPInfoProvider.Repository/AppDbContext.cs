using IPInfoProvider.Types.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace IPInfoProvider.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

         public virtual DbSet<IPDetails> IPDetails { get; set; }

    }
}

