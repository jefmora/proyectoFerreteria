using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models;

public class CambiarEstadoContactoRequestModel
{
    [Range(1, int.MaxValue)]
    public int IdContacto { get; set; }

    [Required, StringLength(20)]
    public string Estado { get; set; } = string.Empty;
}
