using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Browse()
        {
            var results = from e in _eventRepository.GetAllEvents()
                          select e;

            return View(results.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("event/create")]
        public IActionResult Create(EventViewModel eventModel)
        {
            if (ModelState.IsValid)
            {
                var eventEntity = new Event
                {
                    Name = eventModel.EventName,
                    Date = eventModel.EventDate
                };

                for (int i = 1; i <= 10; i++)
                {
                    var ticket = new Ticket
                    {
                        Section = "A1",
                        Row = 1,
                        SeatNumber = i,
                        Price = 125.00M, // TODO: Update event creation to have base price field
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

        // TEMP TEST FUNCTION
        [HttpPost("event/claimticket")]
        public async Task<IActionResult> ClaimTicket(int ticketId)
        {
            if (!UserIsLoggedIn())
            {
                return BadRequest();
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

            return RedirectToAction("Index", "Home");
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
