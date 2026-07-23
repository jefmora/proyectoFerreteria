public class ProductoModel
{
    public int IdProducto { get; set; }

    public string SKU { get; set; }

    public string Nombre { get; set; }

    public decimal Precio { get; set; }

    public int StockActual { get; set; }

    public int StockMinimo { get; set; }

    public bool Estado { get; set; }

    public int IdCategoria { get; set; }
}