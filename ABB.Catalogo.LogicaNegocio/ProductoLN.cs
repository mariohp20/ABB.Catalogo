using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Catalogo.AccesoDatos.Interfaces;
using ABB.Catalogo.Entidades;
using ABB.Catalogo.LogicaNegocio.Interfaces;

namespace ABB.Catalogo.LogicaNegocio
{
    public class ProductoLN : IProductoLN
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoLN(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<List<Producto>> ListarProductosAsync()
        {
            return await _productoRepository.ListarProductosAsync();
        }

        public async Task<bool> InsertarProductoAsync(Producto producto)
        {
            return await _productoRepository.InsertarProductoAsync(producto);
        }

        public async Task<bool> ModificarProductoAsync(Producto producto)
        {
            return await _productoRepository.ModificarProductoAsync(producto);
        }

        public async Task<bool> EliminarProductoAsync(int idProducto)
        {
            return await _productoRepository.EliminarProductoAsync(idProducto);
        }
    }
}