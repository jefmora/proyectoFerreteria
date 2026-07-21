namespace FerreteriaAPI.Models
{
    public class ProductoModel
    {
        public int IdProducto { get; set; }

        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        public int StockActual { get; set; }
    }
}