using Microsoft.AspNetCore.Mvc;
using TigerTix.Web.Data;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;

namespace TigerTix.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly IUserRepository _userRepository;

        private readonly IEventRepository _eventRepository;

        public AppController(IUserRepository userRepository, IEventRepository eventRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/App")]
        public IActionResult Index(User model)
        {
            _userRepository.SaveUser(model);
            _userRepository.SaveAll();

            return View();
        }

        public IActionResult ShowUsers()
        {
            var results = from u in _userRepository.GetAllUsers()
                          select u;

            return View(results.ToList());
        }


        public IActionResult ShowEvents() {
            var events = _eventRepository.GetAllEvents();
            return View(events.ToList());
        }
        
        [HttpGet]
        public IActionResult CreateEvent() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEvent(Event model) {
            if (ModelState.IsValid) {
                _eventRepository.SaveEvent(model);
                _eventRepository.SaveAll();

                return RedirectToAction("ShowEvents");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateEvent(int id) {
            var eventModel = _eventRepository.GetEventByID(id);
            if (eventModel == null) {
                return NotFound();
            }
            return View(eventModel);
        }

        public IActionResult DeleteEvent(int id) {
            var eventModel = _eventRepository.GetEventByID(id);
            if (eventModel == null) {
                return NotFound();
            }

            _eventRepository.DeleteEvent(eventModel);
            _eventRepository.SaveAll();

            return RedirectToAction("ShowEvents");
        }
    }
}
