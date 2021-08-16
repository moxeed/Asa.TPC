using Asa.TPC.Persistence;
using Asa.TPC.Persistence.Block;

namespace Asa.TPC
{
    internal interface IUnitOfWork
    {
        BlockRepository BlockRepository { get; }
        void Commit();
    }
}