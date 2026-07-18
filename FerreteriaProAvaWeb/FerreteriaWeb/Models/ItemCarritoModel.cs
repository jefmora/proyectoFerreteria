namespace Ferreteria.Models
{
    public class ItemCarritoModel
    {
        public ProductoModel Producto { get; set; }

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