using Asa.TPC.Domain;
using Asa.TPC.Persistence.Block;
using Microsoft.EntityFrameworkCore;
using System;
using System.Transactions;

namespace Asa.TPC.Persistence
{
    class Context : DbContext, IPromotableSinglePhaseNotification, IUnitOfWork
    {
        public DbSet<Domain.Block> Blocks { get; private set; }

        public void Initialize()
        { }

        public byte[] Promote()
        {
            return new byte[1];
        }

        public void Rollback(SinglePhaseEnlistment singlePhaseEnlistment)
        {
            singlePhaseEnlistment.Done();
        }

        public void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
        {
            try
            {
                SaveChanges();
                singlePhaseEnlistment.Done();
            }
            catch
            {
                singlePhaseEnlistment.Aborted();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Initial Catalog=TPC;Trusted_connection=true");
            base.OnConfiguring(optionsBuilder);
        }

        public void Commit()
        {
            if (Transaction.Current is not null)
                Transaction.Current.EnlistPromotableSinglePhase(this);
            else
                SaveChanges();
        }

        public BlockRepository BlockRepository => new BlockRepository(this);
    }
}
