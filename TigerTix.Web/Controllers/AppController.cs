using Microsoft.AspNetCore.Mvc;
using TigerTix.Web.Data;
using TigerTix.Web.Data.Entities;
using TigerTix.Web.Models;

namespace TigerTix.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AppController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/App")]
        public IActionResult Index(IndexViewModel model)
        {
            return View();
        }

        public IActionResult ShowUsers()
        {
            var results = from u in _userRepository.GetAllUsers() select u;
            return View(results.ToList());
        }
    }
}
