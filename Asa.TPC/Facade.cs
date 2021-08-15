using Asa.TPC.Domain;
using Asa.TPC.ExternalService;
using Asa.TPC.ExternalService.Block;
using Asa.TPC.Persistence;
using System.Transactions;

namespace Asa.TPC
{
    class Facade
    {
        private readonly Context _context;
        private readonly IUnitOfWork _unitOfWork;

        public Facade()
        {
            _context = new Context();
            _unitOfWork = _context;
        }

        public void Complete()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, options)) 
            {
                var decision = new Decision(100);
                var dependent = Transaction.Current.DependentClone(DependentCloneOption.BlockCommitUntilComplete);

                BlockMoney(decision, dependent);
                _context.OrderRepository.Save(decision);
                _context.SaveChanges();

                transaction.Complete();
            }
        }

        private void BlockMoney(Decision decision, DependentTransaction dependent) 
        {
            using (var innerTransaction = new TransactionScope(dependent))
            {
                var casBlock = new CasBlockCommander(decision);
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);
                innerTransaction.Complete();
            }

            dependent.Complete();
            dependent.Dispose();
        }

        public void ContextAsEnlistment()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var decision = new Decision(100);
                var casBlock = new CasBlockCommander(decision);
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);
                Transaction.Current.EnlistPromotableSinglePhase(_context);
                _context.OrderRepository.Save(decision);
                transaction.Complete();
            }
        }

        public void CompleteUnitOfWork()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var decision = new Decision(1000);
                var casBlock = new CasBlockCommander(decision);
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);

                _unitOfWork.OrderRepository.Save(decision);
                _unitOfWork.Commit();

                transaction.Complete();
            }
        }

        public void AbortUnitOfWork()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var decision = new Decision(1000);
                var casBlock = new CasBlockCommander(decision);
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);

                _unitOfWork.OrderRepository.Save(decision);
                _unitOfWork.Commit();
            }
        }


        public void CasFailUnitOfWork()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var decision = new Decision(1);
                var casBlock = new CasBlockCommander(decision);
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);

                _unitOfWork.OrderRepository.Save(decision);
                _unitOfWork.Commit();

                transaction.Complete();
            }
        }

        public void CompleteAsResourceAndUnitOfWork()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, options))
            {
                var decision = new Decision(1000);
                var casBlock = new CasBlockResource(decision);
                casBlock.Execute();

                _unitOfWork.OrderRepository.Save(decision);

                _unitOfWork.Commit();
                transaction.Complete();
            }
        }
    }
}
