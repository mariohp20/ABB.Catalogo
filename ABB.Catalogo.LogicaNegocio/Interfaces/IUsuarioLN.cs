using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Catalogo.Entidades;

namespace ABB.Catalogo.LogicaNegocio.Interfaces
{
    public interface IUsuarioLN
    {
        Task<List<Usuario>> ListarUsuariosAsync();
        Task<Usuario?> AutenticarUsuarioAsync(string codUsuario, string passwordPlano);
        Task<int> RegistrarUsuarioAsync(Usuario usuario, string passwordPlano);
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario);
        Task<bool> ModificarUsuarioAsync(Usuario usuario, string? nuevaContrasenaPlana);
        Task<List<Rol>> ListarRolesAsync();
    }
}