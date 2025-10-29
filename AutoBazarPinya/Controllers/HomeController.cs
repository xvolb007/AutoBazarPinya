using Microsoft.AspNetCore.Mvc;

namespace AutoBazarPinya.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
