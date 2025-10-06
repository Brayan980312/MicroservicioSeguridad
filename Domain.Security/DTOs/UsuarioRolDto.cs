namespace Domain.Security.DTOs
{
    public class UsuarioRolDto
    {
        /// <summary>Identificador único del UsuarioRol</summary>
        public int UsuarioRolId { get; set; }

        /// <summary>Identificador del Usuario asociado</summary>
        public int UsuarioId { get; set; }

        /// <summary>Identificador del Rol asociado</summary>
        public int RolId { get; set; }
    }
}