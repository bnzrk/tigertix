using Microsoft.AspNetCore.Mvc;

namespace TigerTix.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
