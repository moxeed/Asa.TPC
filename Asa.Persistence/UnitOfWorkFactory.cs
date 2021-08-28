using Asa.TPC;
using Asa.TPC.Persistence;

namespace Asa.Persistence
{
    public static class UnitOfWorkFactory
    {
        public static IUnitOfWork CreateUnitOfWork() => new Context();
    }
}
