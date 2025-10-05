namespace Domain.Security.CustomEntities.Poco
{
    /// <summary>Realiza la inicialización de las propiedades de la entidad AuditoriaPoco.</summary>
    public class AuditoriaPoco
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
