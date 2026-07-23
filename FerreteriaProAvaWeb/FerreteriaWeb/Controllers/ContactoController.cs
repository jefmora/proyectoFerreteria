using FerreteriaWeb.Filter;
using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FerreteriaWeb.Controllers;

public class ContactoController(IHttpClientFactory http, IConfiguration config) : Controller
{
    private HttpClient CrearClienteAutenticado()
    {
        var client = http.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
        return client;
    }

    [HttpGet]
    public IActionResult Crear() => View(new ContactoModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ContactoModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var client = http.CreateClient();
        var response = await client.PostAsJsonAsync(config["Valores:UrlApi"] + "Contacto/RegistrarContacto", model);

        if (response.IsSuccessStatusCode)
        {
            TempData["MensajeExito"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction(nameof(Crear));
        }

        ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
        return View(model);
    }

    [SesionActiva]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        using var client = CrearClienteAutenticado();
        var response = await client.GetAsync(config["Valores:UrlApi"] + "Contacto/ListarContactos");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return RedirectToAction("LoginBasic", "Auth");

        var contactos = response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<List<ContactoModel>>() ?? []
            : [];
        return View(contactos);
    }

    [SesionActiva]
    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        using var client = CrearClienteAutenticado();
        var url = config["Valores:UrlApi"] + "Contacto/";
        var contactoResponse = await client.GetAsync(url + "ObtenerContacto?idContacto=" + id);

        if (contactoResponse.StatusCode == HttpStatusCode.Unauthorized)
            return RedirectToAction("LoginBasic", "Auth");
        if (contactoResponse.StatusCode == HttpStatusCode.NotFound)
            return NotFound();
        if (!contactoResponse.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        var respuestasResponse = await client.GetAsync(url + "ObtenerRespuestas?idContacto=" + id);
        var model = new DetalleContactoViewModel
        {
            Contacto = (await contactoResponse.Content.ReadFromJsonAsync<ContactoModel>())!,
            Respuestas = respuestasResponse.IsSuccessStatusCode
                ? await respuestasResponse.Content.ReadFromJsonAsync<List<RespuestaContactoModel>>() ?? []
                : []
        };
        return View(model);
    }

    [SesionActiva]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Responder(RespuestaContactoModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Respuesta))
        {
            TempData["MensajeError"] = "Debe escribir una respuesta.";
            return RedirectToAction(nameof(Detalle), new { id = model.IdContacto });
        }

        using var client = CrearClienteAutenticado();
        var request = new { model.IdContacto, model.Respuesta, RespondidoPor = HttpContext.Session.GetString("Nombre") ?? "Soporte" };
        var response = await client.PostAsJsonAsync(config["Valores:UrlApi"] + "Contacto/ResponderContacto", request);
        TempData[response.IsSuccessStatusCode ? "MensajeExito" : "MensajeError"] = await response.Content.ReadAsStringAsync();
        return RedirectToAction(nameof(Detalle), new { id = model.IdContacto });
    }

    [SesionActiva]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int idContacto, string estado)
    {
        using var client = CrearClienteAutenticado();
        var response = await client.PutAsJsonAsync(config["Valores:UrlApi"] + "Contacto/CambiarEstadoContacto", new { IdContacto = idContacto, Estado = estado });
        TempData[response.IsSuccessStatusCode ? "MensajeExito" : "MensajeError"] = await response.Content.ReadAsStringAsync();
        return RedirectToAction(nameof(Detalle), new { id = idContacto });
    }

    [SesionActiva]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        using var client = CrearClienteAutenticado();
        var response = await client.DeleteAsync(config["Valores:UrlApi"] + "Contacto/EliminarContacto?idContacto=" + id);
        TempData[response.IsSuccessStatusCode ? "MensajeExito" : "MensajeError"] = await response.Content.ReadAsStringAsync();
        return RedirectToAction(nameof(Index));
    }
}
