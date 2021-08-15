using Asa.TPC.Domain;
using System.Threading.Tasks;

namespace Asa.TPC.Persistence
{
    class DecisionRepository
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
