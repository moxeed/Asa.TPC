using Asa.Persistence.Blocking;
using Asa.TPC.Persistence;

namespace Asa.TPC
{
    public interface IUnitOfWork
    {
        IDecisionRepository OrderRepository { get; }
        IBlockRepository BlockRepository { get; }
        void Commit();
    }
}