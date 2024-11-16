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


        /****************************************************************************************************************************************************************
         * 
         * NOTE: This controller and its actions are for debug purposes now. We can still use it to view pages of things that may not be customer facing or for testing.
         * 
         ****************************************************************************************************************************************************************/
        public AppController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IApplicationUserRepository userRepository, IEventRepository eventRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }            

        public IActionResult Users()
        {
            var results = from u in _userRepository.GetAllUsers()
                          select u;

            return View(results.ToList());
        }
        public IActionResult Events()
        {
            var results = from e in _eventRepository.GetAllEvents()
                          select e;

            return View(results.ToList());
        }
        public IActionResult Tickets()
        {
            var results = from t in _ticketRepository.GetAllTickets()
                          select t;

            return View(results.ToList());
        }
    }
}
