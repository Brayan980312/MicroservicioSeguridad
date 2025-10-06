namespace Domain.Security.CustomEntities.Params
{
    /// <summary>Realiza la inicialización de las propiedades de la entidad ParamsCreacionUsuario.</summary>
    public class ParamsCreacionUsuario
    {
        /// <summary>Número de identificación del Usuario (cédula, DNI, pasaporte, etc.).</summary>
        public required string UsuarioIdentificacion { get; set; }

        /// <summary>Nombres del Usuario.</summary>
        public required string UsuarioNombres { get; set; }

        /// <summary>Apellidos del Usuario.</summary>
        public required string UsuarioApellidos { get; set; }

        /// <summary>Correo electrónico del Usuario.</summary>
        public required string UsuarioCorreo { get; set; }

        /// <summary>Número de celular del Usuario.</summary>
        public required string UsuarioTelefono { get; set; }

        /// <summary>Clave del Usuario (almacenada de forma segura).</summary>
        public required string UsuarioContrasena { get; set; }

        /// <summary>Clave del Usuario (almacenada de forma segura).</summary>
        public required string UsuarioContrasenaConfirmar { get; set; }
    }
}
