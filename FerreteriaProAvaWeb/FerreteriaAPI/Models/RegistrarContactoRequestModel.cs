using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models;

public class RegistrarContactoRequestModel
{
    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Correo { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefono { get; set; }

    [Required, StringLength(200)]
    public string Asunto { get; set; } = string.Empty;

    [Required]
    public string Mensaje { get; set; } = string.Empty;
}
