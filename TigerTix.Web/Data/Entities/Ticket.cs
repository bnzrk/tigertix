namespace TigerTix.Web.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Section { get; set; }
        public int Row { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string? UserOwnerId { get; set; }
        public ApplicationUser UserOwner { get; set; }
        public bool IsReserved { get; set; } = false;
    }
}