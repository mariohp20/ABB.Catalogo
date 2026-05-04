using ABB.Catalogo.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.Catalogo.AccesoDatos.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ListarProductosAsync();
        Task<bool> InsertarProductoAsync(Producto producto);
        Task<bool> ModificarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int idProducto);
    }
}
