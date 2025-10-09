namespace Infrastructure.Security.Persistence.Mapping
{
    using Domain.Security.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario", "Seguridad");

            builder.HasKey(e => e.UsuarioId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.UsuarioIdentificacion)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UsuarioNombreCompleto)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.UsuarioCorreo)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.UsuarioTelefono)
                .IsRequired()
                .HasMaxLength(50)
            .IsUnicode(false);

            builder.Property(e => e.UsuarioClave)
                .IsRequired()
            .IsUnicode(false);            
            
        }

        #endregion
    }
}
