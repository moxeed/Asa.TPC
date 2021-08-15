using Asa.TPC.Domain;
using Asa.TPC.ExternalService;
using Asa.TPC.Persistence;
using System.Threading.Tasks;
using System.Transactions;

namespace Asa.TPC
{
    class Facade
    {
        private readonly OrderRepository _repository;

        public Facade()
        {
            _repository = new OrderRepository();
        }

        public async Task Complete()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var order = new Order();
                await _repository.Save(order);

                var casBlock = new CasBlockCommander();
                Transaction.Current.EnlistVolatile(casBlock, EnlistmentOptions.None);

                //transaction.Complete();
            }
        }
    }
}
