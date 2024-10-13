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
        public IActionResult Index(User model)
        {
            _userRepository.SaveUser(model);
            _userRepository.SaveAll();
            //         User user = new User();
            //user.Id = 0000;
            //user.FirstName = "test";
            //user.LastName = "Smith";
            //         _userRepository.SaveUser(user);
            return View();
        }

        public IActionResult ShowUsers()
        {
            var results = from u in _userRepository.GetAllUsers()
                          select u;
            //if (_userRepository.GetUserByID(0000) == null)
            //{
            //    User user = new User();
            //    user.Id = 0000;
            //    user.FirstName = "John";
            //user.LastName = "Smith";
            //}
            return View(results.ToList());
        }
    }
}
