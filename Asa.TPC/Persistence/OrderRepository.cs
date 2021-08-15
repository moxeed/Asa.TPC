using Asa.TPC.Domain;
using System.Threading.Tasks;

namespace Asa.TPC.Persistence
{
    class OrderRepository
    {
        private readonly Context _context;

        public OrderRepository()
        {
            _context = new Context();
        }

        public async Task<Order> GetOrder(int orderId)
        {
            var order = _context.Orders.FindAsync(orderId);
            return await order;
        }

        public Task Save(Order order)
        {
            _context.Attach(order);
            return _context.SaveChangesAsync();
        }
    }
}
