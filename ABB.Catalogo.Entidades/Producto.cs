using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.Catalogo.Entidades
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public int IdCategoria { get; set; }
        public string NomProducto { get; set; } = string.Empty;
        public string MarcaProducto { get; set; } = string.Empty;
        public string ModeloProducto { get; set; } = string.Empty;
        public string LineaProducto { get; set; } = string.Empty;
        public string? GarantiaProducto { get; set; }
        public decimal? Precio { get; set; }
        public byte[]? Imagen { get; set; }
        public string? DescripcionTecnica { get; set; }

        // Propiedad extendida para mostrar el nombre de la categoría en consultas JOIN
        public string? DescCategoria { get; set; }
    }
}
