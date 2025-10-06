namespace Domain.Security.Entities
{
    using Utilitarios.Entities;
    /// <summary>Realiza la inicialización de las propiedades de la entidad Usuario.</summary>
    public class Usuario : EntidadBase
    {
        /// <summary>Identificador único del Usuario.</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Número de identificación del Usuario (cédula, DNI, pasaporte, etc.).</summary>
        public string UsuarioIdentificacion { get; set; }

        /// <summary>Nombres del Usuario.</summary>
        public string UsuarioNombres { get; set; }

        /// <summary>Apellidos del Usuario.</summary>
        public string UsuarioApellidos { get; set; }

        /// <summary>Correo electrónico del Usuario.</summary>
        public string UsuarioCorreo { get; set; }

        /// <summary>Número de celular del Usuario.</summary>
        public string UsuarioTelefono { get; set; }

        /// <summary>Clave del Usuario (almacenada de forma segura).</summary>
        public string UsuarioClave { get; set; }
    }
}
