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
            using var context = new SqlConnection(
                _config["ConnectionStrings:DefaultConnection"]);

            var response = context.QueryFirstOrDefault<UsuarioResponseModel>(
                """
                SELECT
                    U.IdUsuario AS Consecutivo,
                    U.Identificacion,
                    U.Nombre,
                    U.Correo AS CorreoElectronico,
                    U.PasswordHash AS Contrasenna,
                    U.Estado,
                    CAST(0 AS BIT) AS UsaContrasennaTemp,
                    U.IdRol AS ConsecutivoRol,
                    R.NombreRol,
                    C.Telefono,
                    C.Direccion
                FROM Usuario U
                INNER JOIN Rol R ON U.IdRol = R.IdRol
                LEFT JOIN Cliente C ON U.IdUsuario = C.IdUsuario
                WHERE U.IdUsuario = @Consecutivo;
                """,
                new { Consecutivo = consecutivo });

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound("No se ha encontrado el usuario");
        }


        [HttpGet("ConsultarUsuariosAPI")]
        public IActionResult ConsultarUsuariosAPI()
        {
            using var context = new SqlConnection(
                _config["ConnectionStrings:DefaultConnection"]);

            try
            {
                var response = context.Query<UsuarioResponseModel>(
                    """
                    SELECT
                        U.IdUsuario AS Consecutivo,
                        U.Identificacion,
                        U.Nombre,
                        U.Correo AS CorreoElectronico,
                        U.Estado,
                        CAST(0 AS BIT) AS UsaContrasennaTemp,
                        U.IdRol AS ConsecutivoRol,
                        R.NombreRol
                    FROM Usuario U
                    INNER JOIN Rol R ON U.IdRol = R.IdRol
                    ORDER BY U.Nombre;
                    """).ToList();

                return Ok(response);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ConsultarRolesAPI")]
        public IActionResult ConsultarRolesAPI()
        {
            using var context = new SqlConnection(
                _config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<RolModel>(
                """
                SELECT
                    IdRol AS ConsecutivoRol,
                    NombreRol
                FROM Rol
                ORDER BY IdRol;
                """).ToList();

            return Ok(response);
        }


        [HttpPut("CambiarContrasennaAPI")]
        public IActionResult CambiarContrasennaAPI(
            CambiarContrasennaRequestModel model)
        {
            model.Contrasenna =
                BCrypt.Net.BCrypt.HashPassword(model.Contrasenna);

            using var context = new SqlConnection(
                _config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", model.Consecutivo);
            parameters.Add("@Contrasenna", model.Contrasenna);

            try
            {
                context.Execute(
                    "spActualizarContrasenna",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return Ok("Contraseña actualizada correctamente.");
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("CambiarPerfilAPI")]
        public IActionResult CambiarPerfilAPI(
            CambiarPerfilRequestModel model)
        {
            using var context = new SqlConnection(
                _config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", model.Consecutivo);
            parameters.Add("@Identificacion", model.Identificacion);
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);
            parameters.Add("@Telefono", model.Telefono);
            parameters.Add("@Direccion", model.Direccion);

            var response = context.Execute(
                "spActualizarPerfil",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response > 0)
            {
                return Ok("La información se ha actualizado correctamente");
            }

            return BadRequest("No se ha actualizado su perfil");
        }

        [HttpPut("CambiarEstadoUsuarioAPI")]
        public IActionResult CambiarEstadoUsuarioAPI(int consecutivo)
        {
            using var context = new SqlConnection(
                _config["ConnectionStrings:DefaultConnection"]);

            var response = context.Execute(
                """
                UPDATE Usuario
                SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
                WHERE IdUsuario = @Consecutivo;
                """,
                new { Consecutivo = consecutivo });

            if (response > 0)
                return Ok("Estado del usuario actualizado correctamente");

            return BadRequest("No se pudo actualizar el estado del usuario");
        }

        [HttpPut("CambiarRolUsuarioAPI")]
        public IActionResult CambiarRolUsuarioAPI(CambiarRolRequestModel model)
        {
            using var context = new SqlConnection(
                _config["ConnectionStrings:DefaultConnection"]);

            var response = context.Execute(
                """
                UPDATE Usuario
                SET IdRol = @ConsecutivoRol
                WHERE IdUsuario = @Consecutivo;
                """,
                model);

            if (response > 0)
                return Ok("Rol del usuario actualizado correctamente");

            return BadRequest("No se pudo actualizar el rol del usuario");
        }
    }
}
