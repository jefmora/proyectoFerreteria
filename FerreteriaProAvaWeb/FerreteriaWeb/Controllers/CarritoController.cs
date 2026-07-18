using Microsoft.AspNetCore.Mvc;

namespace Ferreteria.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}