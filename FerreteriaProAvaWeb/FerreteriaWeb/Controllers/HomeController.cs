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
            string url = _config["Valores:UrlApi"] + "Producto/ConsultarProductosDestacados";
            string categoriasUrl = _config["Valores:UrlApi"] + "Producto/ConsultarCategoriasAPI";

            var response = await _httpClient.GetAsync(url);
            var categoriasResponse = await _httpClient.GetAsync(categoriasUrl);

            if (categoriasResponse.IsSuccessStatusCode)
            {
                var categoriasJson = await categoriasResponse.Content.ReadAsStringAsync();
                ViewBag.Categorias = JsonConvert.DeserializeObject<List<CategoriaModel>>(categoriasJson) ?? new List<CategoriaModel>();
            }
            else
            {
                ViewBag.Categorias = new List<CategoriaModel>();
            }

            if (!response.IsSuccessStatusCode)
                return View(new List<ProductoModel>());

            var json = await response.Content.ReadAsStringAsync();

            var productos = JsonConvert.DeserializeObject<List<ProductoModel>>(json) ?? new List<ProductoModel>();

            return View(productos);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
