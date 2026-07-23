namespace FerreteriaAPI.Models
{
    public class ProductoModel
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string? Imagen { get; set; }

        public string? Descripcion { get; set; }
        public int StockActual { get; set; }
        public int IdCategoria { get; set; }
        public string? Categoria { get; set; }

    }
}