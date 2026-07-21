using FerreteriaWeb.Filter;
using FerreteriaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FerreteriaWeb.Controllers
{
    public class AuthController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {

        #region Iniciar Sesión

        [HttpGet]
        public IActionResult LoginBasic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginBasic(UsuarioModel model)
        {
            using var client = _http.CreateClient();
            var url = _config["Valores:UrlApi"] + "Home/IniciarSesionAPI";
            var response = client.PostAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content.ReadFromJsonAsync<UsuarioModel>().Result;

                HttpContext.Session.SetString("Autenticado", "1");
                HttpContext.Session.SetString("Nombre", datos!.Nombre);
                HttpContext.Session.SetInt32("Consecutivo", datos!.Consecutivo);
                HttpContext.Session.SetString("Token", datos!.Token);
                HttpContext.Session.SetInt32("ConsecutivoRol", datos!.ConsecutivoRol);
                HttpContext.Session.SetString("NombreRol", datos!.NombreRol);

                if (datos!.UsaContrasennaTemp)
                    return RedirectToAction("Configuracion", "Usuario");

                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                return View();
            }

            throw new Exception("Error al iniciar sesión");
        }

        #endregion

        #region Registrar Usuario

        [HttpGet]
        public IActionResult RegisterBasic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterBasic(UsuarioModel model)
        {
            using var client = _http.CreateClient();
            var url = _config["Valores:UrlApi"] + "Home/RegistrarAPI";
            var response = client.PostAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                return View();
            }

            throw new Exception("Error al registrar usuario");
        }

        #endregion

        #region Recuperar Acceso

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(UsuarioModel model)
        {
            using var client = _http.CreateClient();
            var url = _config["Valores:UrlApi"] + "Home/RecuperarAccesoAPI";
            var response = client.PostAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("LoginBasic", "Auth");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest
                  || response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                return View();
            }

            throw new Exception("Error al recuperar el acceso");
        }

        #endregion

        #region Cerrar Sesión

        [SesionActivaAttribute]
        [HttpGet]
        public IActionResult Salir()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginBasic", "Auth");
        }

        #endregion

    }
}