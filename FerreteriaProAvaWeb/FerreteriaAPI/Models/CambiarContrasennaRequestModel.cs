using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models
{
    public class CambiarContrasennaRequestModel
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
