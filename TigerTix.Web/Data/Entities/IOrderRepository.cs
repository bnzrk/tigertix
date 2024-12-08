namespace TigerTix.Web.Data.Entities
{
    public interface IOrderRepository
    {
        void SaveOrder(Order order);

        void UpdateOrder(Order order);

        void DeleteOrder(Order order);

        IEnumerable<Order> GetAllOrders();

        Order GetOrderById(int orderId);

        bool SaveAll();
    }
}
