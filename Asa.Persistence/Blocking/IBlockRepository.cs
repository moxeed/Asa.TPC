using System.Threading.Tasks;

namespace Asa.Persistence.Blocking
{
    public interface IBlockRepository
    {
        Task<Core.Domain.Block> GetBlock(int orderId);
        void Save(Core.Domain.Block block);
    }
}