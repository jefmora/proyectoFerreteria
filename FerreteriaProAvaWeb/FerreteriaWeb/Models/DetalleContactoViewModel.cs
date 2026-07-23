namespace FerreteriaWeb.Models;

public class DetalleContactoViewModel
{
    public ContactoModel Contacto { get; set; } = new();
    public List<RespuestaContactoModel> Respuestas { get; set; } = [];
}
