namespace TigerTix.Web.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public ICollection<Ticket> TicketList { get; set; }
    }
}