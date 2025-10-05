namespace Domain.Security.Entities
{
    using Utilitarios.Entities;
    /// <summary>Representa un registro de auditoría dentro del sistema.
    /// Esta entidad captura información de cada petición HTTP procesada por la aplicación, incluyendo datos de entrada, salida, ruta, método y código de respuesta.
    /// </summary>
    public class Auditoria : EntidadBase
    {
        /// <summary>Identificador único del registro de auditoría.</summary>
        public int? AuditoriaId { get; set; }

        /// <summary>Fecha y hora en que se generó la auditoría.</summary>
        public DateTime? AuditoriaFecha { get; set; }

        /// <summary>Método HTTP utilizado en la petición (GET, POST, PUT, DELETE, etc.).</summary>
        public string? AuditoriaMetodoHttp { get; set; }

        /// <summary>Ruta del endpoint solicitado dentro de la API.</summary>
        public string? AuditoriaRuta { get; set; }

        /// <summary>Datos enviados en la petición (body, parámetros o query string).</summary>
        public string? AuditoriaDatosEntrada { get; set; }

        /// <summary>Código de respuesta HTTP devuelto por la API (200, 400, 500, etc.).</summary>
        public int? AuditoriaCodigoRespuesta { get; set; }

        /// <summary>Datos devueltos como respuesta por la API (generalmente JSON o texto plano).</summary>
        public string? AuditoriaDatosSalida { get; set; }
    }
}
