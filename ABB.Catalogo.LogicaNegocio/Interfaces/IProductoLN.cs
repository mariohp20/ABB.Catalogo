using ABB.Catalogo.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABB.Catalogo.LogicaNegocio.Interfaces
{
    public interface IProductoLN
    {
        Task<List<Producto>> ListarProductosAsync();
        Task<bool> InsertarProductoAsync(Producto producto);
        Task<bool> ModificarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int idProducto);
    }
}