using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;
using System.Text.RegularExpressions;

namespace TigerTix.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly IApplicationUserRepository _userRepository;

        private readonly IEventRepository _eventRepository;

        private readonly ITicketRepository _ticketRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AppController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IApplicationUserRepository userRepository, IEventRepository eventRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost("App/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost("App/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Redirect if a user is already logged in
            if (UserIsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            // Ensure valid CUID format in model
            string pattern = @"^C\d{6}$";
            if (!Regex.IsMatch(model.UserName ?? "", pattern))
            {
                ModelState.AddModelError("UserName", "Invalid CUID");
            }

            // Ensure all required fields are filled in model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Attempt to create the new user and sign in on success
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            //var result = await _userManager.CreateAsync(user, model.Password);
            var result = await _userRepository.SaveUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index");
            }

            // Grab all error messages from attempting to create the user and add to model state
            foreach (var error in result.Errors)
            {
                if (error.Code == "DuplicateUserName")
                {
                    // Reword the duplicate username error message
                    ModelState.AddModelError("UserName", "An account with this CUID is already registered.");
                }
                else
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


        public IActionResult ShowEvents() 
        {
            var results = from e in _eventRepository.GetAllEvents()
                          select e;

            return View(results.ToList());
        }

        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost("App/CreateEvent")]
        public IActionResult CreateEvent(EventViewModel eventModel) 
        {
            if (ModelState.IsValid) {
                var eventEntity = new Event {
                   EventName = eventModel.EventName,
                   EventDate = eventModel.EventDate 
                };

                eventEntity.TicketList = new List<Ticket>();
                for (int i = 1; i <= 10; i++) {
                    var ticket = new Ticket {
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

        public IActionResult ShowTickets() {
            var results = from t in _ticketRepository.GetAllTickets()
                          select t;

            return View(results.ToList());
        }

        public IActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost("App/CreateTicket")]
        public IActionResult CreateTicket(TicketViewModel ticketModel) 
        {
            if (ModelState.IsValid) {
                var ticketEntity = new Ticket {
                    CUID = ticketModel.CUID,
                    SeatNumber = ticketModel.SeatNumber,
                    EventId = ticketModel.EventId
                };

                _ticketRepository.SaveTicket(ticketEntity);
                _ticketRepository.SaveAll();
            }
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            if (!UserIsLoggedIn())
                return RedirectToAction("Index");

            ProfileViewModel model = new ProfileViewModel();
            ApplicationUser user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Index");

            model.UserName = user.UserName;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return View(model);
        }

        public IActionResult UserTickets()
        {
            UserTicketViewModel model = new UserTicketViewModel();
            model.DateTime = DateTime.Now;
            model.Number = 1;
            model.Section = "A1";
            model.Row = 1;
            model.EventName = "Event Name";
            model.SeatNumber = 1;

            List<UserTicketViewModel> list = new List<UserTicketViewModel> { model, model, model, model, model };

            return View(list);
        }

        // TODO: Rework to avoid call to database
        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public bool UserIsLoggedIn()
        {
            return User.Identity?.IsAuthenticated == true;
        }

    }
}
