using Asa.Core.Domain;
using Asa.TPC.Persistence;
using System.Threading.Tasks;

namespace Asa.Persistence.Blocking
{
    class BlockRepository : IBlockRepository
    {
        private readonly Context _context;

        public BlockRepository(Context context)
        {
            _context = context;
        }

        public async Task<Block> GetBlock(int orderId)
        {
            var block = _context.Blocks.FindAsync(orderId);
            return await block;
        }

        public void Save(Block block)
        {
            _context.Attach(block);
        }
    }
}
