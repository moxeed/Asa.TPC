using Asa.TPC.Domain;
using System.Threading.Tasks;

namespace Asa.TPC.Persistence.Block
{
    class BlockRepository
    {
        private readonly Context _context;

        public BlockRepository(Context context)
        {
            _context = context;
        }

        public async Task<Domain.Block> GetBlock(int orderId)
        {
            var block = _context.Blocks.FindAsync(orderId);
            return await block;
        }

        public void Save(Domain.Block block)
        {
            _context.Attach(block);
        }
    }
}
