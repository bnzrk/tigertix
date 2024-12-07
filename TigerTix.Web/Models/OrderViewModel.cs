using TigerTix.Web.Data.Entities;

namespace TigerTix.Web.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public List<OrderTicketViewModel> Tickets { get; set; } = new List<OrderTicketViewModel>();
    }
}
