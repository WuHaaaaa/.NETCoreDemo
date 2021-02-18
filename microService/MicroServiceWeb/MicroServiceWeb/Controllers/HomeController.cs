using Microsoft.AspNetCore.Mvc;

namespace MicroServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}