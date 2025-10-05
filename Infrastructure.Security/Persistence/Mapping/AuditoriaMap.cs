namespace Infrastructure.Security.Persistence.Mapping
{
    using Domain.Security.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>Configuración de la entidad Auditoria para el mapeo con la base de datos.</summary>
    public class AuditoriaMap : IEntityTypeConfiguration<Auditoria>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a configurar.</param>
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {
            builder.ToTable("Auditoria", "Auditoria");

            builder.HasComment("Registra las acciones realizadas en la API para fines de auditoría y trazabilidad.");

            builder.HasKey(e => e.AuditoriaId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.AuditoriaId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del registro de auditoría.");

            builder.Property(e => e.AuditoriaFecha)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasComment("Fecha y hora en que se generó la auditoría.");

            builder.Property(e => e.AuditoriaMetodoHttp)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Método HTTP utilizado en la petición (GET, POST, PUT, DELETE, etc.).");

            builder.Property(e => e.AuditoriaRuta)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Ruta o endpoint solicitado dentro de la API.");

            builder.Property(e => e.AuditoriaDatosEntrada)
                .HasColumnType("nvarchar(max)")
                .HasComment("Datos enviados en la petición (body, parámetros o query string).");

            builder.Property(e => e.AuditoriaCodigoRespuesta)
                .IsRequired()
                .HasComment("Código de respuesta HTTP devuelto por la API (200, 400, 500, etc.).");

            builder.Property(e => e.AuditoriaDatosSalida)
                .HasColumnType("nvarchar(max)")
                .HasComment("Datos devueltos como respuesta por la API.");
        }

        #endregion
    }
}
