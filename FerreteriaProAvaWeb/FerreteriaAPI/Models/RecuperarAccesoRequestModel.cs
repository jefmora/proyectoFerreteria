using System.ComponentModel.DataAnnotations;

namespace FerreteriaAPI.Models
{
    public class RecuperarAccesoRequestModel
    {
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;
    }
}
