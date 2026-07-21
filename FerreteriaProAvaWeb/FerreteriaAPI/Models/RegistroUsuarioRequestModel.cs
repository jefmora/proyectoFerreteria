using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models
{
    public class RegistroUsuarioRequestModel
    {
        [Required]
        public string Identificacion { get; set; } = string.Empty;
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;
        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
