namespace TigerTix.Web.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int CUID { get; set; }
        public string TicketEvent { get; set; } = string.Empty;
        public int SeatNumber { get; set; }

    }
}