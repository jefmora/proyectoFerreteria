namespace FerreteriaAPI.Models;

public class RespuestaContactoModel
{
    public int IdRespuesta { get; set; }
    public int IdContacto { get; set; }
    public string Respuesta { get; set; } = string.Empty;
    public string RespondidoPor { get; set; } = string.Empty;
    public DateTime FechaRespuesta { get; set; }
}
