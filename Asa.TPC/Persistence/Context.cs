using Asa.TPC.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Asa.TPC.Persistence
{
    class Context : DbContext
    {
        public DbSet<Order> Orders { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLlocaldb;Initial Catalog=TPC;Trusted_connection=true");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
