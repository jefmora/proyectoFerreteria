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

     private static string CargarPlantilla(string nombreArchivo)
    {
        string ruta = Path.Combine(
            AppContext.BaseDirectory,
            "Templates",
            nombreArchivo);

        return System.IO.File.ReadAllText(ruta);
    }

    private async Task NotificarNuevaSolicitudAsync(RegistrarContactoRequestModel model)
    {
        var destinoSoporte =
            config["ContactoSoporte:CorreoDestino"]
            ?? config["Correos:CuentaGmail"];

        var plantillaSoporte = CargarPlantilla("NuevaSolicitudContacto.html");

        plantillaSoporte = plantillaSoporte
            .Replace("{{NOMBRE}}", Html(model.Nombre))
            .Replace("{{CORREO}}", Html(model.Correo))
            .Replace("{{TELEFONO}}", Html(model.Telefono))
            .Replace("{{ASUNTO}}", Html(model.Asunto))
            .Replace("{{MENSAJE}}", Html(model.Mensaje).Replace("\n", "<br>"));

        await EnviarCorreoSinInterrumpirAsync(
            destinoSoporte,
            "Nueva solicitud de contacto: " + model.Asunto,
            plantillaSoporte);


        var plantillaCliente = CargarPlantilla("ConfirmacionContacto.html");

        plantillaCliente = plantillaCliente
            .Replace("{{NOMBRE}}", Html(model.Nombre))
            .Replace("{{ASUNTO}}", Html(model.Asunto));

        await EnviarCorreoSinInterrumpirAsync(
            model.Correo,
            "Recibimos su solicitud de soporte",
            plantillaCliente);
    }


    private Task NotificarRespuestaAsync(
    ContactoModel contacto,
    ResponderContactoRequestModel respuesta)
    {
        var plantilla = CargarPlantilla("RespuestaContacto.html");

        plantilla = plantilla
            .Replace("{{NOMBRE}}", Html(contacto.Nombre))
            .Replace("{{ASUNTO}}", Html(contacto.Asunto))
            .Replace("{{RESPUESTA}}", Html(respuesta.Respuesta).Replace("\n", "<br>"))
            .Replace("{{RESPONDIDO_POR}}", Html(respuesta.RespondidoPor));

        return EnviarCorreoSinInterrumpirAsync(
            contacto.Correo,
            "Respuesta a su solicitud: " + contacto.Asunto,
            plantilla);
    }


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
