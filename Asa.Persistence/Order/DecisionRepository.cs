using Asa.Core.Domain;
using System.Threading.Tasks;

namespace Asa.TPC.Persistence
{
    class DecisionRepository : IDecisionRepository
    {
        private readonly Context _context;

        public DecisionRepository(Context context)
        {
            _context = context;
        }

        public async Task<Decision> GetDecision(int decisionId)
        {
            var order = _context.Orders.FindAsync(decisionId);
            return await order;
        }

        public void Save(Decision decision)
        {
            _context.Attach(decision);
        }
    }
}
