using Microsoft.AspNetCore.Mvc;

namespace FerreteriaWeb.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}