using FerreteriaWeb.Filter;
using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace FerreteriaWeb.Controllers
{
    [SesionActivaAttribute]
    public class UsuarioController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {

        #region Cambiar Contraseña y Perfil

        [HttpGet]
        public IActionResult Configuracion()
        {
            var consecutivo = HttpContext.Session.GetInt32("Consecutivo")!.Value;

            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _config["Valores:UrlApi"] + "Usuario/ConsultarUsuarioAPI?consecutivo=" + consecutivo;
            var response = client.GetAsync(url).Result;

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound)
            {
                var datos = response.Content.ReadFromJsonAsync<UsuarioModel>().Result;

                return View("Configuracion", datos);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("LoginBasic", "Auth");
            }

            throw new Exception("Error al cambiar la contraseña");
        }

        [HttpPost]
        public IActionResult CambiarContrasenna(UsuarioModel model)
        {
            model.Consecutivo = HttpContext.Session.GetInt32("Consecutivo")!.Value;

            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _config["Valores:UrlApi"] + "Usuario/CambiarContrasennaAPI";
            var response = client.PutAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("LoginBasic", "Auth");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeSeguridad = response.Content.ReadAsStringAsync().Result;
                return View("Configuracion", model);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("LoginBasic", "Auth");
            }

            throw new Exception("Error al cambiar la contraseña");
        }

        [HttpPost]
        public IActionResult CambiarPerfil(UsuarioModel model)
        {
            model.Consecutivo = HttpContext.Session.GetInt32("Consecutivo")!.Value;

            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _config["Valores:UrlApi"] + "Usuario/CambiarPerfilAPI";
            var response = client.PutAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                HttpContext.Session.SetString("Nombre", model!.Nombre);

                ViewBag.MensajePerfil = response.Content.ReadAsStringAsync().Result;
                return View("Configuracion", model);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("LoginBasic", "Auth");
            }

            throw new Exception("Error al cambiar la contraseña");
        }

        #endregion

        #region Módulo de Administración de Usuarios

        [EsAdmin]
        [HttpGet]
        public IActionResult ConsultarUsuarios()
        {
            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _config["Valores:UrlApi"] + "Usuario/ConsultarUsuariosAPI";
            var response = client.GetAsync(url).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content.ReadFromJsonAsync<List<UsuarioModel>>().Result;
                return View("ConsultarUsuarios", datos ?? new List<UsuarioModel>());
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("LoginBasic", "Auth");
            }

            return View("ConsultarUsuarios", new List<UsuarioModel>());
        }

        [EsAdmin]
        [HttpPost]
        public IActionResult CambiarEstadoUsuario(int consecutivo)
        {
            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _config["Valores:UrlApi"] + "Usuario/CambiarEstadoUsuarioAPI?consecutivo=" + consecutivo;
            var response = client.PutAsync(url, null).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TempData["MensajeExito"] = "El estado del usuario ha sido actualizado correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo cambiar el estado del usuario.";
            }

            return RedirectToAction("ConsultarUsuarios");
        }

        [EsAdmin]
        [HttpPost]
        public IActionResult CambiarRolUsuario(int consecutivo, int consecutivoRol)
        {
            using var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _config["Valores:UrlApi"] + "Usuario/CambiarRolUsuarioAPI";
            
            var payload = new { Consecutivo = consecutivo, ConsecutivoRol = consecutivoRol };
            var response = client.PutAsJsonAsync(url, payload).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TempData["MensajeExito"] = "El rol del usuario ha sido actualizado correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo actualizar el rol del usuario.";
            }

            return RedirectToAction("ConsultarUsuarios");
        }

        #endregion
    }
}
