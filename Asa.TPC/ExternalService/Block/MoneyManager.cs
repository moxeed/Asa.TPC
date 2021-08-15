using Asa.TPC.Domain;
using System.Transactions;

namespace Asa.TPC.ExternalService.Block
{
    class CasMoneyManager
    {
        public void Block(Decision decision)
        {
            var commander = new CasBlockCommander(decision);
            Transaction.Current.EnlistVolatile(commander, EnlistmentOptions.None);
        }
    }
}
