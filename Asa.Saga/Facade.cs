using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asa.Saga
{
    class Facade
    {
        public Task MakeADecision(int amount) 
        {
            var sage = new CreateOrderSaga(amount);
            return sage.Run();
        }
    }
}
