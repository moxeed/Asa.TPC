using System;

namespace Asa.Saga
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var facade = new Facade();
            facade.MakeADecision(1);

            Console.WriteLine("Finshed");
        }
    }
}
