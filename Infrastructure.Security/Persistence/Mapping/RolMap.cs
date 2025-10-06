namespace Infrastructure.Security.Persistence.Mapping
{
    using Domain.Security.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RolMap : IEntityTypeConfiguration<Rol>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Rol", "Seguridad");

            builder.HasKey(e => e.RolId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.RolDescripcion)
                .IsRequired()
                .HasMaxLength(100)
            .IsUnicode(false);

            builder.Property(e => e.RolNombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        }

        #endregion
    }
}