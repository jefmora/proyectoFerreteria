using FerreteriaWeb.Filter;
using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

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
            string url = _config["Valores:UrlApi"] + "Producto/ConsultarProductos";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return View(new List<ProductoModel>());

            var json = await response.Content.ReadAsStringAsync();

            var productos = JsonConvert.DeserializeObject<List<ProductoModel>>(json);

            return View(productos);
        }

        #region Módulo de Administración de Productos

        [SesionActiva]
        [EsAdmin]
        [HttpGet]
        public async Task<IActionResult> ConsultarProductosAdmin()
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/ConsultarProductosAdminAPI";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return View(new List<ProductoModel>());

            var json = await response.Content.ReadAsStringAsync();
            var productos = JsonConvert.DeserializeObject<List<ProductoModel>>(json);

            return View(productos ?? new List<ProductoModel>());
        }

        [SesionActiva]
        [EsAdmin]
        [HttpGet]
        public IActionResult RegistrarProducto()
        {
            return View(new ProductoModel());
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> RegistrarProducto(ProductoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/RegistrarProductoAPI";
            var response = await _httpClient.PostAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Producto registrado correctamente.";
                return RedirectToAction("ConsultarProductosAdmin");
            }

            ViewBag.MensajeError = await response.Content.ReadAsStringAsync();
            return View(model);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpGet]
        public async Task<IActionResult> EditarProducto(int idProducto)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/ConsultarProductoPorIdAPI?idProducto=" + idProducto;
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                TempData["MensajeError"] = "No se pudo cargar el producto solicitado.";
                return RedirectToAction("ConsultarProductosAdmin");
            }

            var json = await response.Content.ReadAsStringAsync();
            var producto = JsonConvert.DeserializeObject<ProductoModel>(json);

            return View(producto);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> EditarProducto(ProductoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/ActualizarProductoAPI";
            var response = await _httpClient.PutAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Producto actualizado correctamente.";
                return RedirectToAction("ConsultarProductosAdmin");
            }

            ViewBag.MensajeError = await response.Content.ReadAsStringAsync();
            return View(model);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoProducto(int idProducto)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/CambiarEstadoProductoAPI?idProducto=" + idProducto;
            var response = await _httpClient.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "El estado del producto ha sido cambiado correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo cambiar el estado del producto.";
            }

            return RedirectToAction("ConsultarProductosAdmin");
        }

        #endregion
    }
}