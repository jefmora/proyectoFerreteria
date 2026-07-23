using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FerreteriaWeb.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductoModel> lista = new();

            using var cliente = new HttpClient();

            string url = _configuration["Valores:UrlApi"] + "Producto/ConsultarProductos";

            var respuesta = await cliente.GetAsync(url);

            if (respuesta.IsSuccessStatusCode)
            {
                var json = await respuesta.Content.ReadAsStringAsync();

                lista = JsonSerializer.Deserialize<List<ProductoModel>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })!;
            }

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            using var cliente = new HttpClient();

            string url = _configuration["Valores:UrlApi"] + "Categoria/ConsultarCategorias";

            var respuesta = await cliente.GetAsync(url);

            List<CategoriaModel> categorias = new();

            if (respuesta.IsSuccessStatusCode)
            {
                var json = await respuesta.Content.ReadAsStringAsync();

                categorias = JsonSerializer.Deserialize<List<CategoriaModel>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })!;
            }

            ViewBag.Categorias = new SelectList(categorias, "IdCategoria", "Nombre");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ProductoModel producto)
        {
            using var cliente = new HttpClient();

            string url = _configuration["Valores:UrlApi"] + "Producto/InsertarProducto";

            var contenido = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(producto),
                System.Text.Encoding.UTF8,
                "application/json");

            var respuesta = await cliente.PostAsync(url, contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            using var cliente = new HttpClient();

            string url = _configuration["Valores:UrlApi"] + $"Producto/ConsultarProducto?idProducto={id}";

            var respuesta = await cliente.GetAsync(url);

            if (!respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var json = await respuesta.Content.ReadAsStringAsync();

            var producto = JsonSerializer.Deserialize<ProductoModel>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            using var clienteCategorias = new HttpClient();

            string urlCategorias = _configuration["Valores:UrlApi"] + "Categoria/ConsultarCategorias";

            var respuestaCategorias = await clienteCategorias.GetAsync(urlCategorias);

            List<CategoriaModel> categorias = new();

            if (respuestaCategorias.IsSuccessStatusCode)
            {
                var jsonCategorias = await respuestaCategorias.Content.ReadAsStringAsync();

                categorias = JsonSerializer.Deserialize<List<CategoriaModel>>(jsonCategorias,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })!;
            }

            ViewBag.Categorias = new SelectList(
                categorias,
                "IdCategoria",
                "Nombre",
                producto?.IdCategoria
            );

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ProductoModel producto)
        {
            using var cliente = new HttpClient();

            string url = _configuration["Valores:UrlApi"] + "Producto/ActualizarProducto";

            var contenido = new StringContent(
                JsonSerializer.Serialize(producto),
                Encoding.UTF8,
                "application/json");

            var respuesta = await cliente.PutAsync(url, contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            using var cliente = new HttpClient();

            string url = _configuration["Valores:UrlApi"] + $"Producto/EliminarProducto?idProducto={id}";

            var respuesta = await cliente.DeleteAsync(url);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}