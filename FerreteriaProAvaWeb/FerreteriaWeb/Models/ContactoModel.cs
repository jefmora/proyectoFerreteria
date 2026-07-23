using System.ComponentModel.DataAnnotations;

namespace FerreteriaWeb.Models;

public class ContactoModel
{
    public int IdContacto { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio."), StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio."), EmailAddress(ErrorMessage = "Ingrese un correo válido."), StringLength(150)]
    public string Correo { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El asunto es obligatorio."), StringLength(200)]
    public string Asunto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El mensaje es obligatorio.")]
    public string Mensaje { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
