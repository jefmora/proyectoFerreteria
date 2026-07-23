using Dapper;
using FerreteriaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FerreteriaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IConfiguration _config) : ControllerBase
    {
        [HttpGet("ConsultarUsuarioAPI")]
        public IActionResult ConsultarUsuarioAPI(int consecutivo)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", consecutivo);
            var response = context.QueryFirstOrDefault<UsuarioResponseModel>("spConsultarUsuario", parameters, commandType: CommandType.StoredProcedure);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound("No se ha encontrado el usuario");
        }

        [HttpGet("ConsultarUsuariosAPI")]
        public IActionResult ConsultarUsuariosAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<UsuarioResponseModel>(
                "spConsultarUsuarios",
                commandType: CommandType.StoredProcedure);

            return Ok(response);
        }

        [HttpPut("CambiarEstadoUsuarioAPI")]
        public IActionResult CambiarEstadoUsuarioAPI(int consecutivo)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", consecutivo);
            var response = context.Execute("spCambiarEstadoUsuario", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0 || response == -1)
            {
                return Ok("El estado del usuario ha sido actualizado correctamente.");
            }

            return BadRequest("No se pudo actualizar el estado del usuario.");
        }

        [HttpPut("CambiarRolUsuarioAPI")]
        public IActionResult CambiarRolUsuarioAPI(CambiarRolRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", model.Consecutivo);
            parameters.Add("@ConsecutivoRol", model.ConsecutivoRol);
            var response = context.Execute("spCambiarRolUsuario", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0 || response == -1)
            {
                return Ok("El rol del usuario ha sido actualizado correctamente.");
            }

            return BadRequest("No se pudo actualizar el rol del usuario.");
        }

        [HttpGet("ConsultarRolesAPI")]
        public IActionResult ConsultarRolesAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<dynamic>("spConsultarRoles", commandType: CommandType.StoredProcedure);
            return Ok(response);
        }

        [HttpPut("CambiarContrasennaAPI")]
        public IActionResult CambiarContrasennaAPI(CambiarContrasennaRequestModel model)
        {
            model.Contrasenna = BCrypt.Net.BCrypt.HashPassword(model.Contrasenna);

            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", model.Consecutivo);
            parameters.Add("@Contrasenna", model.Contrasenna);
            parameters.Add("@IndicadorTemp", false);
            var response = context.Execute("spActualizarContrasenna", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0 || response == -1)
            {
                return Ok("La información se ha actualizado correctamente");
            }

            return BadRequest("No se ha actualizado su contraseña");
        }

        [HttpPut("CambiarPerfilAPI")]
        public IActionResult CambiarPerfilAPI(CambiarPerfilRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", model.Consecutivo);
            parameters.Add("@Identificacion", model.Identificacion);
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);
            var response = context.Execute("spActualizarPerfil", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0 || response == -1)
            {
                return Ok("La información se ha actualizado correctamente");
            }

            return BadRequest("No se ha actualizado su perfil");
        }
    }
}
