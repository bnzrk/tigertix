using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Claims;
using TigerTix.Web.Data;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;

namespace TigerTix.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly IUserRepository _userRepository;

        private readonly IEventRepository _eventRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AppController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserRepository userRepository, IEventRepository eventRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            Console.WriteLine("Logged in as: " + user.FirstName + " " + user.LastName);

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost("App/Register")]
        //public IActionResult Register(RegisterViewModel model)
        //{
        //    User user = new User();
        //    user.UserName = model.UserName;
        //    user.FirstName = model.FirstName;
        //    user.LastName = model.LastName;
        //    _userRepository.SaveUser(user);
        //    _userRepository.SaveAll();

        //    return View();
        //}

        [HttpPost("App/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
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
        public IActionResult Login(LoginViewModel model)
        {
            var user = _userRepository.GetUserByUsername(model.UserName);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.UserName);
                Console.WriteLine("Logged in as " + user.FirstName + " " + user.LastName);
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
