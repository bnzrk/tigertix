namespace TigerTix.Web.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int CUID { get; set; } = 0;
        //public string TicketEvent { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        // Foreign key
        public int EventId { get; set; }
        public Event TicketEvent { get; set; }

    }
}