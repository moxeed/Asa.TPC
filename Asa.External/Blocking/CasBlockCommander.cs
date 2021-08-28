using Asa.Core.Domain;
using Asa.TPC.ExternalService.Block;
using System;
using System.Transactions;

namespace Asa.TPC.ExternalService
{
    public class CasBlockCommander : IEnlistmentNotification
    {
        private readonly Decision _decision;
        private readonly CasService _casService;

        public CasBlockCommander(Decision decision)
        {
            _decision = decision;
            _casService = new CasService();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                var blockId = _casService.Block(_decision.TotalAmount).Result;
                _decision.SaveBlockId(blockId);
                Console.WriteLine("Cas Prepared");
                preparingEnlistment.Prepared();
            }
            catch 
            {
                Console.WriteLine("Cas Prepare Roll Back");
                preparingEnlistment.ForceRollback();   
            }
        }
        
        public void InDoubt(Enlistment enlistment)
        {
            Console.WriteLine("Cas InDoubt");
            enlistment.Done();
        }

        public void Commit(Enlistment enlistment)
        {
            Console.WriteLine("Cas Commited");
            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            _casService.Release(1).Wait();
            Console.WriteLine("Cas RolledBack");
            enlistment.Done();
        }
    }
}
