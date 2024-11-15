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


        public EventController(IEventRepository eventRepository, ITicketRepository ticketRepository)
        {
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
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
                    EventName = eventModel.EventName,
                    EventDate = eventModel.EventDate
                };

                for (int i = 1; i <= 10; i++)
                {
                    var ticket = new Ticket
                    {
                        SeatNumber = i,
                        TicketEvent = eventEntity,
                        EventId = eventEntity.Id
                    };

                    eventEntity.TicketList.Add(ticket);
                }

                _eventRepository.SaveEvent(eventEntity);
                _eventRepository.SaveAll();
            }
            return View();
        }

        [HttpGet("event/{eventId}/tickets")]
        public IActionResult Tickets(int eventId)
        {
            Event eventListing = _eventRepository.GetEventByID(eventId);
            if (eventListing == null || eventListing.TicketList == null)
            {
                return NotFound();
            }

            return View(eventListing.TicketList);
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
            ticket.TicketEvent = eventListing;
            ticket.SeatNumber = model.Ticket.SeatNumber;
            ticket.CUID = model.Ticket.CUID;

            eventListing.TicketList.Add(ticket);
            _eventRepository.UpdateEvent(eventListing);
            _eventRepository.SaveAll();
            return RedirectToAction("Tickets", "Event", new { eventId = model.EventId });
        }
    }
}
