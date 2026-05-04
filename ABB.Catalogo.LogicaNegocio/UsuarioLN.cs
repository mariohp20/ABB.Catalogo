using ABB.Catalogo.AccesoDatos.Interfaces;
using ABB.Catalogo.Entidades;
using ABB.Catalogo.LogicaNegocio.Interfaces;
using BCrypt.Net;

namespace ABB.Catalogo.LogicaNegocio
{
    public class UsuarioLN : IUsuarioLN
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // Inyectamos la interfaz del repositorio, no la clase concreta
        public UsuarioLN(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _usuarioRepository.ListarUsuariosAsync();
        }

        public async Task<int> RegistrarUsuarioAsync(Usuario usuario, string passwordPlano)
        {
            // 1. Reglas de Negocio (Ejemplo: validación de longitud)
            if (string.IsNullOrWhiteSpace(passwordPlano) || passwordPlano.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

            if (string.IsNullOrWhiteSpace(usuario.CodUsuario))
                throw new ArgumentException("El código de usuario es obligatorio.");

            // 2. Hasheo criptográfico con BCrypt (Factor de costo 11)
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordPlano, 11);

            // 3. Enviar al repositorio para guardar en SQL Server
            return await _usuarioRepository.InsertarUsuarioAsync(usuario);
        }

        public async Task<Usuario?> AutenticarUsuarioAsync(string codUsuario, string passwordPlano)
        {
            // 1. Obtenemos el usuario por su código
            var usuario = await _usuarioRepository.ObtenerPorCodigoAsync(codUsuario);

            // Si el usuario no existe, rechazamos
            if (usuario == null)
                return null;

            // 2. Verificamos si la contraseña plana ingresada coincide con el Hash de la BD
            bool esValido = BCrypt.Net.BCrypt.Verify(passwordPlano, usuario.PasswordHash);

            if (!esValido)
                return null;

            // 3. SEGURIDAD: Limpiamos el Hash de la memoria antes de enviarlo a la API
            // Así evitamos que el Hash viaje por internet hacia el Frontend (React)
            usuario.PasswordHash = string.Empty;

            return usuario;
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            return await _usuarioRepository.ObtenerUsuarioPorIdAsync(idUsuario);
        }

        public async Task<bool> ModificarUsuarioAsync(Usuario usuario, string? nuevaContrasenaPlana)
        {
            if (!string.IsNullOrWhiteSpace(nuevaContrasenaPlana))
            {
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasenaPlana);
            }
            else
            {
                var usuarioExistente = await _usuarioRepository.ObtenerUsuarioPorIdAsync(usuario.IdUsuario);

                if (usuarioExistente != null)
                {
                    usuario.PasswordHash = usuarioExistente.PasswordHash;
                }
                else
                {
                    return false;
                }
            }

            return await _usuarioRepository.ModificarUsuarioAsync(usuario);
        }

        public async Task<List<Rol>> ListarRolesAsync()
        {
            return await _usuarioRepository.ListarRolesAsync();
        }
    }
}