namespace Domain.Security.DTOs
{
    public class UsuarioDto
    {
        /// <summary>Identificador único del Usuario.</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Número de identificación del Usuario (cédula, DNI, pasaporte, etc.).</summary>
        public string UsuarioIdentificacion { get; set; }

        /// <summary>Nombre completo del Usuario.</summary>
        public string UsuarioNombreCompleto { get; set; }

        /// <summary>Correo electrónico del Usuario.</summary>
        public string UsuarioCorreo { get; set; }

        /// <summary>Número de celular del Usuario.</summary>
        public string UsuarioTelefono { get; set; }

        /// <summary>Clave del Usuario (almacenada de forma segura).</summary>
        public string UsuarioClave { get; set; }
    }
}
