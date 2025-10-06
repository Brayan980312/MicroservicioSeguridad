using Domain.Security.Entities;

namespace Domain.Security.DTOs
{

    public class InicioSesionDto
    {
        /// <summary>Token generado por el JWT para la usabilidad en el sistema.</summary>
        public string TokenJWT { get; set; }

        /// <summary>Identificador único del Usuario.</summary>
        public int UsuarioId { get; set; }

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

        /// <summary>Lista de roles asociados al usuario.</summary>
        public List<UsuarioRol> Roles { get; set; }
    }
}