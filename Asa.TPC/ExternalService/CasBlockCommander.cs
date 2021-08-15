using System;
using System.Transactions;

namespace Asa.TPC.ExternalService
{
    class CasBlockCommander : IEnlistmentNotification
    {
        public void Commit(Enlistment enlistment)
        {
            enlistment.Done();
            Console.WriteLine("Cas Commited");
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
            Console.WriteLine("Cas InDoubt");
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
            Console.WriteLine("Cas Prepared");
        }

        public void Rollback(Enlistment enlistment)
        {
            enlistment.Done();
            Console.WriteLine("Cas RolledBack");
        }
    }
}
