using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.Catalogo.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string CodUsuario { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;
        public int IdRol { get; set; }

        // Propiedad extendida útil para mostrar el nombre del rol en el frontend
        public string? DesRol { get; set; }
    }
}
