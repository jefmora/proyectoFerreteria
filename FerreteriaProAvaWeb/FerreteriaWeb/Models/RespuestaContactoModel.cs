using System.ComponentModel.DataAnnotations;

namespace FerreteriaWeb.Models;

public class RespuestaContactoModel
{
    public int IdRespuesta { get; set; }
    public int IdContacto { get; set; }

    [Required(ErrorMessage = "La respuesta es obligatoria.")]
    public string Respuesta { get; set; } = string.Empty;

    public string RespondidoPor { get; set; } = string.Empty;
    public DateTime FechaRespuesta { get; set; }
}
