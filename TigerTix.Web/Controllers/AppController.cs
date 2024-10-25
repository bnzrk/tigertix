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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("App/Register")]
        public IActionResult Register(User user)
        {
            _userRepository.SaveUser(user);
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
            var results = from e in _eventRepository.GetAllEvents()
                          select e;

            return View(results.ToList());
        }

        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost("App/CreateEvent")]
        public IActionResult CreateEvent(Event eventListing) {
            if (ModelState.IsValid) {
                _eventRepository.SaveEvent(eventListing);
                _eventRepository.SaveAll();
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("App/Login")]
        public IActionResult Login(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.UserName);
                return RedirectToAction("Login");
            }

            return View();
        }

        User GetSessionUser()
        {
            var sessionUsername = HttpContext.Session.GetString("Username");
            if (sessionUsername == null)
                return null;

            var sessionUser = _userRepository.GetUserByUsername(sessionUsername);
            return sessionUser;
        }

    }
}
