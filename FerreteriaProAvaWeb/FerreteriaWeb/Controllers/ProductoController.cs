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
        public async Task<IActionResult> RegistrarProducto()
        {
            ViewBag.Categorias = await ObtenerCategoriasAsync();
            return View(new ProductoModel());
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> RegistrarProducto(ProductoModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await ObtenerCategoriasAsync();
                return View(model);
            }

            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/RegistrarProductoAPI";
            var response = await _httpClient.PostAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Producto registrado correctamente.";
                return RedirectToAction("ConsultarProductosAdmin");
            }

            ViewBag.Categorias = await ObtenerCategoriasAsync();
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

            ViewBag.Categorias = await ObtenerCategoriasAsync();
            return View(producto);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> EditarProducto(ProductoModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await ObtenerCategoriasAsync();
                return View(model);
            }

            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/ActualizarProductoAPI";
            var response = await _httpClient.PutAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Producto actualizado correctamente.";
                return RedirectToAction("ConsultarProductosAdmin");
            }

            ViewBag.Categorias = await ObtenerCategoriasAsync();
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

        #region Módulo de Gestión de Categorías

        [SesionActiva]
        [EsAdmin]
        [HttpGet]
        public async Task<IActionResult> ConsultarCategorias()
        {
            var categorias = await ObtenerCategoriasAsync();
            return View(categorias);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpGet]
        public IActionResult RegistrarCategoria()
        {
            return View(new CategoriaModel());
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> RegistrarCategoria(CategoriaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/RegistrarCategoriaAPI";
            var response = await _httpClient.PostAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Categoría registrada correctamente.";
                return RedirectToAction("ConsultarCategorias");
            }

            ViewBag.MensajeError = await response.Content.ReadAsStringAsync();
            return View(model);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpGet]
        public async Task<IActionResult> EditarCategoria(int idCategoria)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/ConsultarCategoriaPorIdAPI?idCategoria=" + idCategoria;
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                TempData["MensajeError"] = "No se pudo cargar la información de la categoría.";
                return RedirectToAction("ConsultarCategorias");
            }

            var json = await response.Content.ReadAsStringAsync();
            var categoria = JsonConvert.DeserializeObject<CategoriaModel>(json);
            return View(categoria);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> EditarCategoria(CategoriaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/ActualizarCategoriaAPI";
            var response = await _httpClient.PutAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Categoría actualizada correctamente.";
                return RedirectToAction("ConsultarCategorias");
            }

            ViewBag.MensajeError = await response.Content.ReadAsStringAsync();
            return View(model);
        }

        [SesionActiva]
        [EsAdmin]
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoCategoria(int idCategoria)
        {
            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = _config["Valores:UrlApi"] + "Producto/CambiarEstadoCategoriaAPI?idCategoria=" + idCategoria;
            var response = await _httpClient.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "El estado de la categoría ha sido cambiado correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo cambiar el estado de la categoría.";
            }

            return RedirectToAction("ConsultarCategorias");
        }

        #endregion

        #region Métodos Privados Auxiliares

        private async Task<List<CategoriaModel>> ObtenerCategoriasAsync()
        {
            string url = _config["Valores:UrlApi"] + "Producto/ConsultarCategoriasAPI";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<CategoriaModel>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CategoriaModel>>(json) ?? new List<CategoriaModel>();
        }

        #endregion
    }
}