using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;

namespace TigerTix.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;

        private readonly ITicketRepository _ticketRepository;

        private readonly UserManager<ApplicationUser> _userManager;


        public EventController(UserManager<ApplicationUser> userManager, IEventRepository eventRepository, ITicketRepository ticketRepository)
        {
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
            _userManager = userManager;
        }

        [HttpGet("/event/{eventId}")]
        public IActionResult Index(int eventId)
        {
            Event eventListing = _eventRepository.GetEventByID(eventId);
            if (eventListing == null)
            {
                return NotFound();
            }

            var eventViewModel = new EventViewModel
            {
                Id = eventListing.Id,
                Name = eventListing.Name,
                Description = eventListing.Description,
                Date = eventListing.Date,
                BasePrice = eventListing.BasePrice
            };

            List<Ticket> tickets = _eventRepository.GetEventUnownedTickets(eventId);
            var ticketsViewModel = tickets.Select(ticket => new TicketViewModel
            {
                Id = ticket.Id,
                Section = ticket.Section,
                Row = ticket.Row,
                SeatNumber = ticket.SeatNumber,
                Price = ticket.Price
            }).ToList();

            var model = new EventDetailsViewModel
            {
                Event = eventViewModel,
                Tickets = ticketsViewModel
            };

            return View(model);
        }

        [HttpGet("/event/browse")]
        public IActionResult Browse(BrowseEventsViewModel browseEventsModel)
        {
            var results = _eventRepository.SearchEvents(browseEventsModel.SearchParameters);
    
            browseEventsModel.Events = results;

            return View(browseEventsModel);
        }

        [HttpGet("/event/create")]
        public IActionResult Create()
        {
            Console.WriteLine("Creat page.");
            return View();
        }

        [HttpPost("/event/create")]
        public IActionResult Create(EventViewModel eventModel)
        {
            Console.WriteLine("Creat page POST.");
            if (ModelState.IsValid)
            {
                var eventEntity = new Event
                {
                    Name = eventModel.Name,
                    Description = eventModel.Description,
                    Date = eventModel.Date,
                    BasePrice= eventModel.BasePrice
                };

                for (int i = 1; i <= 10; i++)
                {
                    var ticket = new Ticket
                    {
                        Section = "A1",
                        Row = 1,
                        SeatNumber = i,
                        Price = eventEntity.BasePrice,
                        Event = eventEntity,
                        EventId = eventEntity.Id
                    };

                    eventEntity.Tickets.Add(ticket);
                }

                _eventRepository.SaveEvent(eventEntity);
                _eventRepository.SaveAll();
            }
            return RedirectToAction("Browse", "Event");
        }

        [HttpGet("event/{eventId}/tickets")]
        public IActionResult Tickets(int eventId)
        { 
            List<Ticket> tickets = _eventRepository.GetEventUnownedTickets(eventId);
            if (tickets.Count == 0 || tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        [HttpGet("/event/{eventId}/createticket")]
        public IActionResult CreateTicket(int eventId)  
        {
            // FIXME: Does not properly handle error when a non-existing event is attempted
            Event eventListing = _eventRepository.GetEventByID(eventId);
            if (eventListing == null)
            {
                return NotFound();
            }

            var model = new CreateTicketViewModel
            {
                EventId = eventId
            };

            return View(model);
        }

        // FIXME: Rework the model to allow for proper validation
        [HttpPost("/event/{eventId}/createticket")]
        public IActionResult CreateTicket(int eventId, CreateTicketViewModel model)
        {
            Event eventListing = _eventRepository.GetEventByID(eventId);
            if (eventListing == null)
            {
                return NotFound();
            }

            Ticket ticket = new Ticket();
            ticket.EventId = eventId;
            ticket.Event = eventListing;
            ticket.SeatNumber = model.Ticket.SeatNumber;
            ticket.Row = model.Ticket.Row;
            ticket.Section = model.Ticket.Section;
            ticket.Price = model.Ticket.Price;

            eventListing.Tickets.Add(ticket);
            _eventRepository.UpdateEvent(eventListing);
            _eventRepository.SaveAll();
            return RedirectToAction("Tickets", "Event", new { eventId = model.EventId });
        }

        // TEMP TEST
        [HttpPost("/event/claimticket")]
        public async Task<IActionResult> ClaimTicket([FromQuery] int eventId, [FromQuery] int ticketId)
        {
            if (!UserIsLoggedIn())
            {
                return RedirectToAction("Login", "User");
            }

            ApplicationUser currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            Ticket claimedTicket = _ticketRepository.GetTicketByID(ticketId);
            if (claimedTicket == null)
            {
                return NotFound();
            }

            currentUser.Tickets.Add(claimedTicket);
            _ticketRepository.UpdateTicket(claimedTicket);
            _ticketRepository.SaveAll();

            await _userManager.UpdateAsync(currentUser);

            return RedirectToAction("Index", "Event", new { eventId } );
        }

        // TEMP TEST
        [HttpPost("/event/claimanyticket")]
        public async Task<IActionResult> ClaimAnyTicket([FromQuery] int eventId)
        {
            // Do nothing for now

            return RedirectToAction("Index", "Event", new { eventId });
        }

        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        protected bool UserIsLoggedIn()
        {
            return User.Identity?.IsAuthenticated == true;
        }
    }
}
