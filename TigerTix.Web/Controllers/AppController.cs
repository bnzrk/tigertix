using Microsoft.AspNetCore.Mvc;
using TigerTix.Web.Data;
using TigerTix.Web.Models;

namespace TigerTix.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly TigerTixContext _context;

        public AppController(TigerTixContext context)
        {
            _context = context;
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
            var results = from u in _context.Users select u;
            return View(results.ToList());
        }
    }
}
