namespace Infrastructure.Security.Persistence.Mapping
{
    using Domain.Security.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UsuarioRolMap : IEntityTypeConfiguration<UsuarioRol>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.ToTable("UsuarioRol", "Seguridad");

            builder.HasKey(e => e.UsuarioRolId);

            builder.Ignore(e => e.Id);

        }

        #endregion
    }
}
