namespace ABB.Catalogo.API.DTOs
{
    public class RegistroUsuarioDTO
    {
        public string CodUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public int IdRol { get; set; }
    }
}