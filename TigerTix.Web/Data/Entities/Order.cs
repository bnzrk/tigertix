namespace TigerTix.Web.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public bool IsActive { get; set; } = false;
    }
}
