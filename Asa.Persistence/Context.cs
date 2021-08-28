using Asa.Core.Domain;
using Asa.Persistence.Blocking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Transactions;

namespace Asa.TPC.Persistence
{
    class Context : DbContext, IPromotableSinglePhaseNotification, IUnitOfWork
    {
        public DbSet<Decision> Orders { get; private set; }
        public DbSet<Block> Blocks { get; private set; }

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

        public IDecisionRepository OrderRepository => new DecisionRepository(this);
        public IBlockRepository BlockRepository => new BlockRepository(this);
    }
}
