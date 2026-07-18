using System.Collections.Generic;
using System.Linq;

namespace Ferreteria.Models
{
    public class CarritoModel
    {
        public List<ItemCarritoModel> Items { get; set; } = new();

        public decimal Subtotal => Items.Sum(x => x.Total);

        public decimal IVA => Subtotal * 0.13m;

        public decimal Total => Subtotal + IVA;
    }
}