using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public Event GetEventByID(int eventId)
        {
            var tigerTixEvent = (from e in _context.Events where e.Id == eventId select e).FirstOrDefault();
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

        public IEnumerable<Event> SearchEvents(EventSearchParameters parameters)
        {
            IEnumerable<Event> query = GetAllEvents();

            if (!string.IsNullOrEmpty(parameters.Name)) {
                query = query.Where(e => e.Name.Contains(parameters.Name));
            }

            if (!string.IsNullOrEmpty(parameters.Description)) {
                query = query.Where(e => e.Description.Contains(parameters.Description));
            }

            if (parameters.Date.HasValue) {
                DateTime searchDate = parameters.Date.Value.Date;
                DateTime nextDay = searchDate.AddDays(1);

                query = query.Where(e => e.Date >= searchDate && e.Date < nextDay);
            }

            if (parameters.BasePrice.HasValue) {
                query = query.Where(e => e.BasePrice >= parameters.BasePrice.Value);
            }

            return query.ToList();
        }

        public List<Ticket> GetEventTickets(int eventId)
        {
            return _context.Events
            .Where(e => e.Id == eventId)
            .SelectMany(e => e.Tickets)
            .ToList();
        }
        public List<Ticket> GetEventUnownedTickets(int eventId)
        {
            return _context.Events
            .Where(e => e.Id == eventId)
            .SelectMany(e => e.Tickets)
            .Where(t => t.UserOwner == null)
            .ToList();
        }
    }
}