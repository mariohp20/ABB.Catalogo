namespace ABB.Catalogo.API.DTOs
{
    public class UsuarioResponseDTO
    {
        public int IdUsuario { get; set; }
        public string CodUsuario { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string? DesRol { get; set; }

        // Aquí guardaremos el JWT que generaremos más adelante
        public string Token { get; set; } = string.Empty;
    }
}