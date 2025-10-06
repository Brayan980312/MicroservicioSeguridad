namespace Domain.Security.CustomEntities.Params
{
    /// <summary>Realiza la inicialización de las propiedades de la entidad ParamsIniciarSesion.</summary>
    public class ParamsIniciarSesion
    {
        /// <summary>Número de identificación del Usuario (cédula, DNI, pasaporte, etc.).</summary>
        public required string UsuarioIdentificacion { get; set; }

        /// <summary>Clave del Usuario (almacenada de forma segura).</summary>
        public required string UsuarioClave { get; set; }
    }
}
