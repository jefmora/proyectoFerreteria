using FerreteriaWeb.Filter;
using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FerreteriaWeb.Controllers
{
    [SesionActiva]
    [EsAdmin]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public AdminController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            int totalUsuarios = 0;
            int totalProductos = 0;
            int stockBajo = 0;

            try
            {
                var respUsuarios = await client.GetAsync(_config["Valores:UrlApi"] + "Usuario/ConsultarUsuariosAPI");
                if (respUsuarios.IsSuccessStatusCode)
                {
                    var lista = await respUsuarios.Content.ReadFromJsonAsync<List<UsuarioModel>>();
                    totalUsuarios = lista?.Count ?? 0;
                }

                var respProd = await client.GetAsync(_config["Valores:UrlApi"] + "Producto/ConsultarProductosAdminAPI");
                if (respProd.IsSuccessStatusCode)
                {
                    var productos = await respProd.Content.ReadFromJsonAsync<List<ProductoModel>>();
                    totalProductos = productos?.Count ?? 0;
                    stockBajo = productos?.Count(p => p.StockActual <= p.StockMinimo) ?? 0;
                }
            }
            catch
            {
                // Fallback
            }

            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.TotalProductos = totalProductos;
            ViewBag.StockBajo = stockBajo;

            return View();
        }
    }
}
