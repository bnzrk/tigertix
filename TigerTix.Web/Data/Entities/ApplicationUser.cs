using Microsoft.AspNetCore.Identity;

namespace TigerTix.Web.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Order> Orders { get; set; }
        public Order? ActiveOrder => Orders.FirstOrDefault(o => o.IsActive);
    }
}
