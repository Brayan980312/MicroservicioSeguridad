namespace Domain.Security.Entities
{
    using Utilitarios.Entities;

    /// <summary>Realiza la inicialización de las propiedades de la entidad UsuarioRol.</summary>
    public class UsuarioRol : EntidadBase
    {
        /// <summary>Identificador único del UsuarioRol</summary>
        public int? UsuarioRolId { get; set; }

        /// <summary>Identificador del Usuario asociado</summary>
        public int? UsuarioId { get; set; }

        /// <summary>Identificador del Rol asociado</summary>
        public int? RolId { get; set; }
    }
}
