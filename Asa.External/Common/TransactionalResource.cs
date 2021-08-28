using System.Transactions;

namespace Asa.TPC.ExternalService.Common
{
    public abstract class TransactionalResource : IEnlistmentNotification
    {
        public void Execute()
        {
            if (Transaction.Current is not null)
                Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
            else
                Prepare();
        }

        void IEnlistmentNotification.InDoubt(Enlistment enlistment)
        {
            //Log Data
            enlistment.Done();
        }

        protected abstract void Commit();
        void IEnlistmentNotification.Commit(Enlistment enlistment)
        {
            Commit();
            enlistment.Done();
        }

        protected abstract void Rollback();
        void IEnlistmentNotification.Rollback(Enlistment enlistment)
        {
            Rollback();
            enlistment.Done();
        }

        protected abstract void Prepare();
        void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                Prepare();
                preparingEnlistment.Prepared();
            }
            catch
            {
                preparingEnlistment.ForceRollback();
            }
        }
    }
}
