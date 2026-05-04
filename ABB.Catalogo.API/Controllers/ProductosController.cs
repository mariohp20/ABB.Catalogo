using ABB.Catalogo.LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ABB.Catalogo.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Catalogo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //Este endpoint requiere un Token JWT
    public class ProductosController : ControllerBase
    {
        private readonly IProductoLN _productoLN;

        public ProductosController(IProductoLN productoLN)
        {
            _productoLN = productoLN;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _productoLN.ListarProductosAsync();
            return Ok(productos);
        }

        // POST: /api/Productos 
        // Regla: Solo Admin (Rol 1) y Almacenero (Rol 3) pueden registrar nuevos motores o transformadores.
        [HttpPost]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> Post([FromBody] Producto producto)
        {
            var exito = await _productoLN.InsertarProductoAsync(producto);
            if (exito) return Ok(new { mensaje = "Producto registrado con éxito en el catálogo" });
            return BadRequest("Error al intentar registrar el producto");
        }

        // PUT: /api/Productos/5 
        // Regla: Solo Admin (Rol 1) y Almacenero (Rol 3) pueden modificar datos técnicos o precios.
        [HttpPut("{id}")]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> Put(int id, [FromBody] Producto producto)
        {
            producto.IdProducto = id; // Aseguramos la integridad del ID recibido en la URL
            var exito = await _productoLN.ModificarProductoAsync(producto);
            if (exito) return Ok(new { mensaje = "Datos del producto actualizados correctamente" });
            return BadRequest("Error al actualizar el producto");
        }

        // DELETE: /api/Productos/5 
        // Regla Estricta: SOLO el Administrador (Rol 1) tiene privilegios para eliminar un producto del sistema.
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {
            var exito = await _productoLN.EliminarProductoAsync(id);
            if (exito) return Ok(new { mensaje = "Producto eliminado permanentemente de la base de datos" });
            return BadRequest("Error al intentar eliminar el producto");
        }
    }
}