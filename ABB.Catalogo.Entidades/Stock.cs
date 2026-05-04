using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.Catalogo.Entidades
{
    public class Stock
    {
        public int IdProducto { get; set; }
        public int StockItems { get; set; }
        public int? PuntoRepo { get; set; }
        public decimal? PrecioVenta { get; set; }
    }
}
