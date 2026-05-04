using System.Data;
using ABB.Catalogo.AccesoDatos.Interfaces;
using ABB.Catalogo.Entidades;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ABB.Catalogo.AccesoDatos.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _cadenaConexion;

        public UsuarioRepository(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("cnnSql")
                ?? throw new ArgumentNullException("La cadena de conexión no está configurada.");
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var resultado = await db.QueryAsync<Usuario>(
                "ListarUsuarios",
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        public async Task<Usuario?> ObtenerPorCodigoAsync(string codUsuario)
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var parametros = new DynamicParameters();
            parametros.Add("@ParamUsuario", codUsuario);

            return await db.QueryFirstOrDefaultAsync<Usuario>(
                "paUsuario_ObtenerPorCodigo",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertarUsuarioAsync(Usuario usuario)
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var parametros = new DynamicParameters();

            parametros.Add("@PasswordHash", usuario.PasswordHash);
            parametros.Add("@CodUsuario", usuario.CodUsuario);
            parametros.Add("@Nombres", usuario.Nombres);
            parametros.Add("@IdRol", usuario.IdRol);

            return await db.ExecuteScalarAsync<int>(
                "paUsuario_insertar",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ModificarUsuarioAsync(Usuario usuario)
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var parametros = new DynamicParameters();

            parametros.Add("@IdUsuario", usuario.IdUsuario);
            parametros.Add("@CodUsuario", usuario.CodUsuario);
            parametros.Add("@PasswordHash", usuario.PasswordHash);
            parametros.Add("@Nombres", usuario.Nombres);
            parametros.Add("@IdRol", usuario.IdRol);

            var filasAfectadas = await db.ExecuteAsync(
                "paUsuario_Modificar",
                parametros,
                commandType: CommandType.StoredProcedure);

            return filasAfectadas > 0;
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var parametros = new DynamicParameters();
            parametros.Add("@ParamUsuario", idUsuario);
            return await db.QueryFirstOrDefaultAsync<Usuario>("paUsuario_BuscaUserId", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Rol>> ListarRolesAsync()
        {
            using IDbConnection db = new SqlConnection(_cadenaConexion);
            var resultado = await db.QueryAsync<Rol>("ListarRol", commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }
    }
}
