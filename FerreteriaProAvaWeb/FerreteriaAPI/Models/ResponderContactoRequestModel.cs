using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models;

public class ResponderContactoRequestModel
{
    [Range(1, int.MaxValue)]
    public int IdContacto { get; set; }

    [Required]
    public string Respuesta { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string RespondidoPor { get; set; } = string.Empty;
}
