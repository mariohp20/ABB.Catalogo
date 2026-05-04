using System.Data;
using ABB.Catalogo.AccesoDatos.Interfaces;
using ABB.Catalogo.Entidades;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ABB.Catalogo.AccesoDatos.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string _cadenaConexion;

        public ProductoRepository(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("cnnSql")
                ?? throw new ArgumentNullException("La cadena de conexión no está configurada.");
        }

        public async Task<List<Producto>> ListarProductosAsync()
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var resultado = await db.QueryAsync<Producto>(
                "paListarProductos",
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        public async Task<bool> InsertarProductoAsync(Producto producto)
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var parametros = new DynamicParameters();
            parametros.Add("@IdCategoria", producto.IdCategoria);
            parametros.Add("@NomProducto", producto.NomProducto);
            parametros.Add("@MarcaProducto", producto.MarcaProducto);
            parametros.Add("@ModeloProducto", producto.ModeloProducto);
            parametros.Add("@LineaProducto", producto.LineaProducto);
            parametros.Add("@GarantiaProducto", producto.GarantiaProducto);
            parametros.Add("@Precio", producto.Precio);
            parametros.Add("@DescripcionTecnica", producto.DescripcionTecnica);

            var filasAfectadas = await db.ExecuteAsync("paProducto_Insertar", parametros, commandType: CommandType.StoredProcedure);
            return filasAfectadas > 0;
        }

        public async Task<bool> ModificarProductoAsync(Producto producto)
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var parametros = new DynamicParameters();
            parametros.Add("@IdProducto", producto.IdProducto);
            parametros.Add("@IdCategoria", producto.IdCategoria);
            parametros.Add("@NomProducto", producto.NomProducto);
            parametros.Add("@MarcaProducto", producto.MarcaProducto);
            parametros.Add("@ModeloProducto", producto.ModeloProducto);
            parametros.Add("@LineaProducto", producto.LineaProducto);
            parametros.Add("@GarantiaProducto", producto.GarantiaProducto);
            parametros.Add("@Precio", producto.Precio);
            parametros.Add("@DescripcionTecnica", producto.DescripcionTecnica);

            var filasAfectadas = await db.ExecuteAsync("paProducto_Modificar", parametros, commandType: CommandType.StoredProcedure);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarProductoAsync(int idProducto)
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var parametros = new DynamicParameters();
            parametros.Add("@IdProducto", idProducto);

            var filasAfectadas = await db.ExecuteAsync("paProducto_Eliminar", parametros, commandType: CommandType.StoredProcedure);
            return filasAfectadas > 0;
        }
    }
}
