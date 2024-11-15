using Microsoft.EntityFrameworkCore;

namespace TigerTix.Web.Data.Entities
{
    // FIXME: Returned events need to explicity include the ticket list because it is in another table or the returned event's ticket list will be empty
    public class EventRepository : IEventRepository
    {
        private readonly TigerTixContext _context;

        public EventRepository(TigerTixContext context)
        {
            _context = context;
        }

        public void SaveEvent(Event tigerTixEvent)
        {
            _context.Add(tigerTixEvent);
            _context.SaveChanges();
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var events = from e in _context.Events select e;
            return events.ToList();
        }

        public Event GetEventByID(int eventID)
        {
            // Get first matching event entry or default if not found
            var tigerTixEvent = (from e in _context.Events
                                 where e.Id == eventID
                                 select e)
                                .Include(e => e.TicketList) // Include the ticket list
                                .FirstOrDefault();
            return tigerTixEvent;
        }

        public void UpdateEvent(Event tigerTixEvent)
        {
            _context.Update(tigerTixEvent);
            _context.SaveChanges();
        }

        public void DeleteEvent(Event tigerTixEvent)
        {
            _context.Remove(tigerTixEvent);
            _context.SaveChanges();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}