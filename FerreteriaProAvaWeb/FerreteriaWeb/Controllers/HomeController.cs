using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FerreteriaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public HomeController(IConfiguration config, IHttpClientFactory factory)
        {
            _config = config;
            _httpClient = factory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            string url = _config["Valores:UrlApi"] + "Producto/ConsultarProductos";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return View(new List<ProductoModel>());

            var json = await response.Content.ReadAsStringAsync();

            var productos = JsonConvert.DeserializeObject<List<ProductoModel>>(json);

            return View(productos);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}