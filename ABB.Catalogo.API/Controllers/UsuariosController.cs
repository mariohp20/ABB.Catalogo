using ABB.Catalogo.API.DTOs;
using ABB.Catalogo.Entidades;
using ABB.Catalogo.LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ABB.Catalogo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioLN _usuarioLN;
        private readonly IConfiguration _config;

        // Inyectamos la Lógica de Negocio y la Configuración
        public UsuariosController(IUsuarioLN usuarioLN, IConfiguration config)
        {
            _usuarioLN = usuarioLN;
            _config = config;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] RegistroUsuarioDTO dto)
        {
            try
            {
                var nuevoUsuario = new Usuario
                {
                    CodUsuario = dto.CodUsuario,
                    Nombres = dto.Nombres,
                    IdRol = dto.IdRol
                };

                var idGenerado = await _usuarioLN.RegistrarUsuarioAsync(nuevoUsuario, dto.Password);
                return Ok(new { Mensaje = "Usuario registrado con éxito", IdUsuario = idGenerado });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _usuarioLN.AutenticarUsuarioAsync(dto.CodUsuario, dto.Password);

            if (usuario == null)
                return Unauthorized(new { Error = "Credenciales incorrectas" });

            // 1. Generación del Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Guardamos datos básicos en el token (Claims)
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.CodUsuario),
                    new Claim(ClaimTypes.Role, usuario.IdRol.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _config["Jwt:Issuer"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 2. Retornamos la respuesta con el Token real
            var response = new UsuarioResponseDTO
            {
                IdUsuario = usuario.IdUsuario,
                CodUsuario = usuario.CodUsuario,
                Nombres = usuario.Nombres,
                DesRol = usuario.DesRol,
                Token = tokenString
            };

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioLN.ListarUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetUsuarioPorId(int id)
        {
            var usuario = await _usuarioLN.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound("Usuario no encontrado");
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] RegistroUsuarioDTO dto)
        {
            var usuario = new Usuario
            {
                IdUsuario = id,
                CodUsuario = dto.CodUsuario,
                Nombres = dto.Nombres,
                IdRol = dto.IdRol
            };

            var exito = await _usuarioLN.ModificarUsuarioAsync(usuario, dto.Password);
            if (exito) return Ok(new { mensaje = "Usuario actualizado correctamente en el sistema" });

            return BadRequest("Error al intentar actualizar el usuario");
        }

        // GET: api/Usuarios/roles
        [HttpGet("roles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _usuarioLN.ListarRolesAsync();
            return Ok(roles);
        }
    }
}