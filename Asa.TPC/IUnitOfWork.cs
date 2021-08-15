using Asa.TPC.Persistence;
using Asa.TPC.Persistence.Block;

namespace Asa.TPC
{
    internal interface IUnitOfWork
    {
        DecisionRepository OrderRepository { get; }
        BlockRepository BlockRepository { get; }
        void Commit();
    }
}