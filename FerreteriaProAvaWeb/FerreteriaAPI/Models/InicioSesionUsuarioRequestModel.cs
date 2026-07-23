using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models
{
    public class InicioSesionUsuarioRequestModel
    {
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;
        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
