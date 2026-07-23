using Dapper;
using FerreteriaAPI.Models;
using FerreteriaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace FerreteriaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactoController(IConfiguration config, IUtilesService utiles) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("RegistrarContacto")]
    public async Task<IActionResult> RegistrarContacto(RegistrarContactoRequestModel model)
    {
        using var context = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        var parameters = new DynamicParameters(model);

        try
        {
            context.Execute("SP_RegistrarContacto", parameters, commandType: CommandType.StoredProcedure);
            await NotificarNuevaSolicitudAsync(model);
            return Ok("Su mensaje fue enviado correctamente. Le responderemos pronto.");
        }
        catch (SqlException)
        {
            return BadRequest("No fue posible registrar su mensaje.");
        }
    }

    [Authorize]
    [HttpGet("ListarContactos")]
    public IActionResult ListarContactos()
    {
        using var context = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        var contactos = context.Query<ContactoModel>("SP_ListarContactos", commandType: CommandType.StoredProcedure);
        return Ok(contactos);
    }

    [Authorize]
    [HttpGet("ObtenerContacto")]
    public IActionResult ObtenerContacto(int idContacto)
    {
        using var context = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        var parameters = new DynamicParameters();
        parameters.Add("@IdContacto", idContacto);
        var contacto = context.QueryFirstOrDefault<ContactoModel>("SP_ObtenerContacto", parameters, commandType: CommandType.StoredProcedure);
        return contacto is null ? NotFound("No se encontró el contacto solicitado.") : Ok(contacto);
    }

    [Authorize]
    [HttpGet("ObtenerRespuestas")]
    public IActionResult ObtenerRespuestas(int idContacto)
    {
        using var context = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        var parameters = new DynamicParameters();
        parameters.Add("@IdContacto", idContacto);
        var respuestas = context.Query<RespuestaContactoModel>("SP_ObtenerRespuestas", parameters, commandType: CommandType.StoredProcedure);
        return Ok(respuestas);
    }

    [Authorize]
    [HttpPost("ResponderContacto")]
    public async Task<IActionResult> ResponderContacto(ResponderContactoRequestModel model)
    {
        using var context = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        var parameters = new DynamicParameters(model);

        try
        {
            var contacto = context.QueryFirstOrDefault<ContactoModel>(
                "SP_ObtenerContacto",
                new { model.IdContacto },
                commandType: CommandType.StoredProcedure);

            if (contacto is null)
                return NotFound("No se encontró el contacto solicitado.");

            context.Execute("SP_ResponderContacto", parameters, commandType: CommandType.StoredProcedure);
            await NotificarRespuestaAsync(contacto, model);
            return Ok("Respuesta registrada correctamente.");
        }
        catch (SqlException)
        {
            return BadRequest("No fue posible registrar la respuesta.");
        }
    }

    [Authorize]
    [HttpPut("CambiarEstadoContacto")]
    public IActionResult CambiarEstadoContacto(CambiarEstadoContactoRequestModel model)
    {
        using var context = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        var parameters = new DynamicParameters(model);
        var filas = context.Execute("SP_CambiarEstadoContacto", parameters, commandType: CommandType.StoredProcedure);
        return filas == 0 ? NotFound("No se encontró el contacto solicitado.") : Ok("Estado actualizado correctamente.");
    }

    [Authorize]
    [HttpDelete("EliminarContacto")]
    public IActionResult EliminarContacto(int idContacto)
    {
        using var context = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        var parameters = new DynamicParameters();
        parameters.Add("@IdContacto", idContacto);
        var filas = context.Execute("SP_EliminarContacto", parameters, commandType: CommandType.StoredProcedure);
        return filas == 0 ? NotFound("No se encontró el contacto solicitado.") : Ok("Contacto eliminado correctamente.");
    }

    private async Task NotificarNuevaSolicitudAsync(RegistrarContactoRequestModel model)
    {
        var destinoSoporte = config["ContactoSoporte:CorreoDestino"] ?? config["Correos:CuentaGmail"];
        var asunto = "Nueva solicitud de contacto: " + model.Asunto;
        var detalle = $"<p>Se recibió una nueva solicitud de contacto.</p>" +
                      $"<p><strong>Nombre:</strong> {Html(model.Nombre)}<br>" +
                      $"<strong>Correo:</strong> {Html(model.Correo)}<br>" +
                      $"<strong>Teléfono:</strong> {Html(model.Telefono)}</p>" +
                      $"<p><strong>Asunto:</strong> {Html(model.Asunto)}</p><p>{Html(model.Mensaje).Replace("\n", "<br>")}</p>";

        await EnviarCorreoSinInterrumpirAsync(destinoSoporte, asunto, detalle);
        await EnviarCorreoSinInterrumpirAsync(
            model.Correo,
            "Recibimos su solicitud de soporte",
            $"<p>Hola {Html(model.Nombre)},</p><p>Recibimos su solicitud sobre <strong>{Html(model.Asunto)}</strong>. Nuestro equipo le responderá pronto.</p>");
    }

    private Task NotificarRespuestaAsync(ContactoModel contacto, ResponderContactoRequestModel respuesta) =>
        EnviarCorreoSinInterrumpirAsync(
            contacto.Correo,
            "Respuesta a su solicitud: " + contacto.Asunto,
            $"<p>Hola {Html(contacto.Nombre)},</p><p>Respondimos su solicitud <strong>{Html(contacto.Asunto)}</strong>:</p>" +
            $"<p>{Html(respuesta.Respuesta).Replace("\n", "<br>")}</p><p>Atentamente,<br>{Html(respuesta.RespondidoPor)}</p>");

    private async Task EnviarCorreoSinInterrumpirAsync(string? destinatario, string asunto, string cuerpo)
    {
        if (string.IsNullOrWhiteSpace(destinatario))
            return;

        try
        {
            await utiles.EnviarCorreoAsync(destinatario, asunto, cuerpo);
        }
        catch
        {
            // La solicitud/respuesta ya quedó guardada; un problema SMTP no debe revertirla.
        }
    }

    private static string Html(string? valor) => WebUtility.HtmlEncode(valor ?? string.Empty);
}
