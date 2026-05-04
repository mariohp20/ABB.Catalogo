using ABB.Catalogo.Entidades;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ABB.Catalogo.AccesoDatos.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ListarUsuariosAsync();
        Task<Usuario?> ObtenerPorCodigoAsync(string codUsuario);
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario);
        Task<int> InsertarUsuarioAsync(Usuario usuario);
        Task<bool> ModificarUsuarioAsync(Usuario usuario);
        Task<List<Rol>> ListarRolesAsync();
    }
}
