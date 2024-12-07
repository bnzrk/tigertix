namespace TigerTix.Web.Data.Entities
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TigerTixContext _context;

        public OrderRepository(TigerTixContext context)
        {
            _context = context;
        }

        public void SaveOrder(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            var orders = from o in _context.Orders select o;
            return orders.ToList();
        }

        public Order GetOrderById(int orderId)
        {
            var order = (from o in _context.Orders where o.Id == orderId select o).FirstOrDefault();
            return order;
        }

        public void UpdateOrder(Order order)
        {
            _context.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            _context.Remove(order);
            _context.SaveChanges();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
