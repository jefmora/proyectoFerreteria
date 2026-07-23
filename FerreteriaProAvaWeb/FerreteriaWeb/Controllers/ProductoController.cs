using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FerreteriaWeb.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public ProductoController(IConfiguration config, IHttpClientFactory factory)
        {
            _config = config;
            _httpClient = factory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            string url = _config["Valores:UrlApi"] + "Producto/ConsultarProductosDestacados";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return View(new List<ProductoModel>());

            var json = await response.Content.ReadAsStringAsync();

            var productos = JsonConvert.DeserializeObject<List<ProductoModel>>(json);

            return View(productos);
        }
        public async Task<IActionResult> Detalle(int id)
        {
            string url = _config["Valores:UrlApi"] + $"Producto/ConsultarProducto?idProducto={id}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var json = await response.Content.ReadAsStringAsync();

            var producto = JsonConvert.DeserializeObject<ProductoModel>(json);

            return View(producto);
        }
    }
}