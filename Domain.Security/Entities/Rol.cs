namespace Domain.Security.Entities
{
    using Utilitarios.Entities;
    /// <summary>Realiza la inicialización de las propiedades de la entidad Rol.</summary>
    public class Rol : EntidadBase
    {
        /// <summary>Identificador único del Rol.</summary>
        public int RolId { get; set; }

        /// <summary>Nombre del Rol.</summary>
        public string RolNombre { get; set; }

        /// <summary>Descripción del Rol.</summary>
        public string RolDescripcion { get; set; }

        /// <summary>Estado del Rol (activo o inactivo).</summary>
        public bool RolEstado { get; set; }
    }
}
