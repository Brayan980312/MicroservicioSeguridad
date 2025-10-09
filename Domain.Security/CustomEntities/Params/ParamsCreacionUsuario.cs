namespace Domain.Security.CustomEntities.Params
{
    /// <summary>Realiza la inicialización de las propiedades de la entidad ParamsCreacionUsuario.</summary>
    public class ParamsCreacionUsuario
    {
        /// <summary>Número de identificación del Usuario (cédula, DNI, pasaporte, etc.).</summary>
        public required string UsuarioIdentificacion { get; set; }

        /// <summary>Nombre y apellido del Usuario.</summary>
        public required string UsuarioNombreCompleto { get; set; }

        /// <summary>Correo electrónico del Usuario.</summary>
        public required string UsuarioCorreo { get; set; }

        /// <summary>Número de celular del Usuario.</summary>
        public required string UsuarioTelefono { get; set; }

        /// <summary>Clave del Usuario (almacenada de forma segura).</summary>
        public required string UsuarioClave { get; set; }

        /// <summary>Clave del Usuario (almacenada de forma segura).</summary>
        public required string UsuarioClaveConfirmar { get; set; }
    }
}
