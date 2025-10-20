namespace Infrastructure.Security.Persistence.Mapping
{
    using Domain.Security.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configuración de la entidad <see cref="RefreshTokens"/> para su mapeo en la base de datos.
    /// </summary>
    public class RefreshTokenMap : IEntityTypeConfiguration<RefreshTokens>
    {
        #region Métodos

        /// <summary>
        /// Método para realizar la configuración de la entidad según la estructura en la base de datos.
        /// </summary>
        /// <param name="builder">Entidad a configurar.</param>
        public void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            builder.ToTable("RefreshTokens", "Seguridad");

            builder.HasKey(e => e.RefreshTokenId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.UsuarioId)
                .IsRequired();

            builder.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.ExpiraEn)
                .IsRequired();

            builder.Property(e => e.Revocado)
                .IsRequired()
                .HasDefaultValue(false);
        }

        #endregion
    }
}
