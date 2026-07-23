namespace FerreteriaWeb.Models
{
    public class ProductoModel
    {
        public int IdProducto { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string? Imagen { get; set; }
        public string? Descripcion { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public bool Estado { get; set; } = true;
        public int IdCategoria { get; set; } = 1;
        public string NombreCategoria { get; set; } = string.Empty;
        public string? Categoria { get; set; }
    }
}