using Domain.Security.Entities;

namespace Domain.Security.DTOs
{

    public class InicioSesionDto
    {
        /// <summary>Token JWT generado para la autenticación del usuario.</summary>
        public string AccessToken { get; set; }

        /// <summary>Token utilizado para renovar el JWT sin volver a iniciar sesión.</summary>
        public string RefreshToken { get; set; }

        /// <summary>Fecha y hora de expiración del JWT.</summary>
        public DateTime ExpiraEn { get; set; }

        /// <summary>Identificador único del Usuario.</summary>
        public int UsuarioId { get; set; }

        /// <summary>Número de identificación del Usuario (cédula, DNI, pasaporte, etc.).</summary>
        public string UsuarioIdentificacion { get; set; }

        /// <summary>Nombre completo del Usuario.</summary>
        public string UsuarioNombreCompleto { get; set; }

        /// <summary>Correo electrónico del Usuario.</summary>
        public string UsuarioCorreo { get; set; }

        /// <summary>Número de celular del Usuario.</summary>
        public string UsuarioTelefono { get; set; }

        /// <summary>Lista de roles asociados al usuario.</summary>
        public List<UsuarioRol> Roles { get; set; }
    }
}