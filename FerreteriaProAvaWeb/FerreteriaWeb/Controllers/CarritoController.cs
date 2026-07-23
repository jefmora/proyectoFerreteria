using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FerreteriaWeb.Helpers;

namespace FerreteriaWeb.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public CarritoController(IConfiguration config, IHttpClientFactory factory)
        {
            _config = config;
            _httpClient = factory.CreateClient();
        }

        public IActionResult Index()
        {
            var carrito = HttpContext.Session.GetObject<CarritoModel>("Carrito");

            if (carrito == null)
                carrito = new CarritoModel();

            return View(carrito);
        }

        public async Task<IActionResult> Agregar(int id, int cantidad)
        {
            if (cantidad < 1)
                cantidad = 1;
            string url = _config["Valores:UrlApi"] + "Producto/ConsultarProductos";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");

            var json = await response.Content.ReadAsStringAsync();

            var productos = JsonConvert.DeserializeObject<List<ProductoModel>>(json);

            var producto = productos.FirstOrDefault(x => x.IdProducto == id);

            if (producto == null)
                return RedirectToAction("Index", "Home");

            var carrito = HttpContext.Session.GetObject<CarritoModel>("Carrito") ?? new CarritoModel();

            var item = carrito.Items.FirstOrDefault(x => x.Producto.IdProducto == id);

            if (item != null)
            {
                item.Cantidad += cantidad;
            }
            else
            {
                carrito.Items.Add(new ItemCarritoModel
                {
                    Producto = producto,
                    Cantidad = cantidad
                });
            }

            HttpContext.Session.SetObject("Carrito", carrito);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Aumentar(int id)
        {
            var carrito = HttpContext.Session.GetObject<CarritoModel>("Carrito");

            if (carrito != null)
            {
                var item = carrito.Items.FirstOrDefault(x => x.Producto.IdProducto == id);

                if (item != null)
                {
                    item.Cantidad++;
                    HttpContext.Session.SetObject("Carrito", carrito);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Disminuir(int id)
        {
            var carrito = HttpContext.Session.GetObject<CarritoModel>("Carrito");

            if (carrito != null)
            {
                var item = carrito.Items.FirstOrDefault(x => x.Producto.IdProducto == id);

                if (item != null)
                {
                    item.Cantidad--;

                    if (item.Cantidad <= 0)
                        carrito.Items.Remove(item);

                    HttpContext.Session.SetObject("Carrito", carrito);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var carrito = HttpContext.Session.GetObject<CarritoModel>("Carrito");

            if (carrito != null)
            {
                var item = carrito.Items.FirstOrDefault(x => x.Producto.IdProducto == id);

                if (item != null)
                {
                    carrito.Items.Remove(item);
                    HttpContext.Session.SetObject("Carrito", carrito);
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult Vaciar()
        {
            HttpContext.Session.Remove("Carrito");

            return RedirectToAction("Index");
        }
    }
}