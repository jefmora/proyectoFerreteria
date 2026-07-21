namespace FerreteriaWeb.Models
{
    public class ItemCarritoModel
    {
        public ProductoModel Producto { get; set; } = new ProductoModel();

        public int Cantidad { get; set; }

        public decimal Total
        {
            get
            {
                return Producto.Precio * Cantidad;
            }
        }
    }
}