using System;
using System.Threading.Tasks;

namespace Asa.TPC
{
    class Program
    {
        static async Task Main()
        {
            var facade = new Facade();
            //await facade.CompleteAsync();
            facade.CompleteAsResourceAndUnitOfWork();

            Console.Read();
        }
    }
}
