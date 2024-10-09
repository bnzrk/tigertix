using Microsoft.AspNetCore.Mvc;
using TigerTix.Web.Models;

namespace TigerTix.Web.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/App")]
        public IActionResult Index(IndexViewModel model)
        {
            return View();
        }
    }
}
