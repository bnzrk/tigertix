namespace TigerTix.Web.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}