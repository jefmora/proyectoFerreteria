using Dapper;
using FerreteriaAPI.Models;
using FerreteriaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FerreteriaAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IConfiguration _config, IUtilesService _utiles) : ControllerBase
    {
        [HttpPost("RegistrarAPI")]
        public IActionResult RegistrarAPI(RegistroUsuarioRequestModel model)
        {
            // La contraseña se cifra aquí (no en el proyecto Web)
            model.Contrasenna = BCrypt.Net.BCrypt.HashPassword(model.Contrasenna);

            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", model.Identificacion);
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);
            parameters.Add("@Contrasenna", model.Contrasenna);

            try
            {
                context.Execute(
                    "spRegistrarUsuario",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return Ok("Usuario registrado correctamente.");
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("IniciarSesionAPI")]
        public IActionResult IniciarSesionAPI(InicioSesionUsuarioRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);
            parameters.Add("@Contrasenna", model.Contrasenna);

            var response = context.QueryFirstOrDefault<UsuarioResponseModel>(
                "spIniciarSesionUsuario",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response != null &&
                BCrypt.Net.BCrypt.Verify(model.Contrasenna, response.Contrasenna))
            {
                response.Token = _utiles.GenerarToken(response.Consecutivo);
                return Ok(response);
            }

            return NotFound("No se ha validado su información correctamente.");
        }

        [HttpPost("RecuperarAccesoAPI")]
        public async Task<IActionResult> RecuperarAccesoAPI(RecuperarAccesoRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);

            var response = context.QueryFirstOrDefault<UsuarioResponseModel>(
                "spValidarCorreo",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response == null)
                return NotFound("No se ha validado su información correctamente.");

            // Generar contraseña temporal
            var temporal = _utiles.GenerarContrasena();
            var temporalCifrada = BCrypt.Net.BCrypt.HashPassword(temporal);

            parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", response.Consecutivo);
            parameters.Add("@Contrasenna", temporalCifrada);

            var update = context.Execute(
                "spActualizarContrasenna",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (update > 0)
            {
                string ruta = Path.Combine(AppContext.BaseDirectory, "Templates", "RecuperarAcceso.html");
                string plantilla = System.IO.File.ReadAllText(ruta);

                plantilla = plantilla.Replace("{{TEMPORAL}}", temporal);
                plantilla = plantilla.Replace("{{NOMBRE}}", response.Nombre);

                await _utiles.EnviarCorreoAsync(
                    model.CorreoElectronico,
                    "Recuperación de acceso",
                    plantilla);

                return Ok(response);
            }

            return BadRequest("No se ha recuperado su acceso, intente nuevamente más tarde.");
        }
    }
}