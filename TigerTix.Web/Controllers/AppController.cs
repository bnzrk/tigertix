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

        public IActionResult ShowUsers()
        {
            var results = from u in _userRepository.GetAllUsers()
                          select u;

            return View(results.ToList());
        }

        //public IActionResult CreateTicket()
        //{
        //    return View();
        //}

        //[HttpPost("App/CreateTicket")]
        //public IActionResult CreateTicket(CreateTicketViewModel ticketModel) 
        //{
        //    //if (ModelState.IsValid) {
        //    //    var ticketEntity = new Ticket {
        //    //        CUID = ticketModel.CUID,
        //    //        SeatNumber = ticketModel.SeatNumber,
        //    //        EventId = ticketModel.EventId
        //    //    };

        //    //    _ticketRepository.SaveTicket(ticketEntity);
        //    //    _ticketRepository.SaveAll();
        //    //}
        //    return View();
        //}
    }
}
