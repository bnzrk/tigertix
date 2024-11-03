namespace TigerTix.Web.Data.Entities
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TigerTixContext _context;

        public TicketRepository(TigerTixContext context)
        {
            _context = context;
        }

        public void SaveTicket(Ticket tigerTixTicket)
        {
            _context.Add(tigerTixTicket);
            _context.SaveChanges();
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            var tix = from t in _context.Tickets select t;
            return tix.ToList();
        }

        public Ticket GetTicketByID(int ticketID)
        {
            // Get first matching ticket entry or default if not found
            var tigerTixTicket = (from t in _context.Tickets where t.Id == ticketID select t).FirstOrDefault();
            return tigerTixTicket;
        }

        public void UpdateTicket(Ticket tigerTixTicket)
        {
            _context.Update(tigerTixTicket);
            _context.SaveChanges();
        }

        public void DeleteTicket(Ticket tigerTixTicket)
        {
            _context.Remove(tigerTixTicket);
            _context.SaveChanges();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}